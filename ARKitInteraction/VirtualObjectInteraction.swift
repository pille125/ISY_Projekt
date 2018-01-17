/*
See LICENSE folder for this sample’s licensing information.

Abstract:
Coordinates movement and gesture interactions with virtual objects.
*/

import UIKit
import ARKit

/// - Tag: VirtualObjectInteraction
class VirtualObjectInteraction: NSObject, UIGestureRecognizerDelegate {
    
    enum Status: String {
        case none = "none"
        case select = "select"
        case trigger = "trigger"
        case rotate = "rotate"
    }
    
    /// Developer setting to translate assuming the detected plane extends infinitely.
    let translateAssumingInfinitePlane = true
    
    // Track object status
    var status = Status.none
    
    var showInfo = false
    
    /// The scene view to hit test against when moving virtual content.
    let sceneView: VirtualObjectARView
    
    /**
     The object that has been most recently intereacted with.
     The `selectedObject` can be moved at any time with the tap gesture.
     */
    var selectedObject: VirtualObject?
    
    /// The object that is tracked for use by the pan and rotation gestures.
    private var trackedObject: VirtualObject? {
        didSet {
            guard trackedObject != nil else { return }
            selectedObject = trackedObject
        }
    }
    
    /// The tracked screen position used to update the `trackedObject`'s position in `updateObjectToCurrentTrackingPosition()`.
    private var currentTrackingPosition: CGPoint?

    init(sceneView: VirtualObjectARView) {
        self.sceneView = sceneView
        super.init()
        
        let panGesture = ThresholdPanGesture(target: self, action: #selector(didPan(_:)))
        panGesture.delegate = self
        
        
        let rotationGesture = UIRotationGestureRecognizer(target: self, action: #selector(didRotate(_:)))
        rotationGesture.delegate = self
        
        let tapGesture = UITapGestureRecognizer(target: self, action: #selector(didTap(_:)))
        
        let doubleTapGesture = UITapGestureRecognizer(target: self, action: #selector(didDoubleTap(_:)))
        doubleTapGesture.numberOfTapsRequired = 2
        
        let longPressGesture = UILongPressGestureRecognizer(target: self, action: #selector(didLongPress(_:)))
        
        
        
        
        // Add gestures to the `sceneView`.
        sceneView.addGestureRecognizer(panGesture)
        sceneView.addGestureRecognizer(rotationGesture)
        sceneView.addGestureRecognizer(tapGesture)
        sceneView.addGestureRecognizer(doubleTapGesture)
        sceneView.addGestureRecognizer(longPressGesture)
        
    }
    
    // MARK: - Gesture Actions
    
    @objc
    func didPan(_ gesture: ThresholdPanGesture) {
        switch status {
        case Status.select:
            switch gesture.state {
            case .began:
                // Check for interaction with a new object.
                if let object = objectInteracting(with: gesture, in: sceneView) {
                    trackedObject = object
                }
                
            case .changed where gesture.isThresholdExceeded:
                guard let object = trackedObject else { return }
                let translation = gesture.translation(in: sceneView)
                
                let currentPosition = currentTrackingPosition ?? CGPoint(sceneView.projectPoint(object.position))
                
                // The `currentTrackingPosition` is used to update the `selectedObject` in `updateObjectToCurrentTrackingPosition()`.
                currentTrackingPosition = CGPoint(x: currentPosition.x + translation.x, y: currentPosition.y + translation.y)
                
                gesture.setTranslation(.zero, in: sceneView)
                
            case .changed:
                // Ignore changes to the pan gesture until the threshold for displacment has been exceeded.
                break
                
            default:
                // Clear the current position tracking.
                currentTrackingPosition = nil
                trackedObject = nil
            }
        case Status.rotate:
            print("rotated by \(gesture.translation(in: sceneView).y/10000)")
            trackedObject?.eulerAngles.y -= Float(gesture.translation(in: sceneView).y/10000)
        case .none:
            //do nothing
            break
        case .trigger:
            //do nothing
            break
        }
    }
    

    /**
     If a drag gesture is in progress, update the tracked object's position by
     converting the 2D touch location on screen (`currentTrackingPosition`) to
     3D world space.
     This method is called per frame (via `SCNSceneRendererDelegate` callbacks),
     allowing drag gestures to move virtual objects regardless of whether one
     drags a finger across the screen or moves the device through space.
     - Tag: updateObjectToCurrentTrackingPosition
     */
    @objc
    func updateObjectToCurrentTrackingPosition() {
        guard let object = trackedObject, let position = currentTrackingPosition else { return }
        translate(object, basedOn: position, infinitePlane: translateAssumingInfinitePlane)
    }

    /// - Tag: didRotate
    @objc
    func didRotate(_ gesture: UIRotationGestureRecognizer) {
        guard gesture.state == .changed else { return }
        
        /*
         - Note:
          For looking down on the object (99% of all use cases), we need to subtract the angle.
          To make rotation also work correctly when looking from below the object one would have to
          flip the sign of the angle depending on whether the object is above or below the camera...
         */
        print("rotategesture")
        print(Float(gesture.rotation))
        trackedObject?.eulerAngles.y -= Float(gesture.rotation)
        
        gesture.rotation = 0
    }
    
    @objc
    func didTap(_ gesture: UITapGestureRecognizer) {
        let touchLocation = gesture.location(in: sceneView)
        
        
        let hitResults = sceneView.hitTest(touchLocation, options: [:])
        // check that we clicked on at least one object
        if hitResults.count > 0 {
            // retrieved the first clicked object
            let result = hitResults[0]
            if let tappedObject = sceneView.virtualObject(at: touchLocation) {
                if tappedObject.childNode(withName: "rotateButton", recursively: true) == result.node {
                    print("rotate pressed")
                    status = Status.rotate
                    return
                }
                if tappedObject.childNode(withName: "infoButton", recursively: true) == result.node {
                    print("info pressed")
                    if showInfo == true {
                        showInfo = false
                        print("info hidden")
                        tappedObject.parent?.childNode(withName: "infoText", recursively: true)?.isHidden = true
                        
                    }else {
                        showInfo = true
                        print("info visible")
                        tappedObject.parent?.childNode(withName: "infoText", recursively: true)?.isHidden = false
                    }
                    return
                }
                if tappedObject.childNode(withName: "copyButton", recursively: true) == result.node {
                    print("copy pressed")
                    return
                }
        
            }
            
            // get its material
            let materials = result.node.geometry!.materials//.firstMaterial!
            
            // highlight it
            SCNTransaction.begin()
            for material in materials {
                if (material.emission.contents as! UIColor == UIColor.blue) {
                    material.emission.contents = UIColor.black
                    status = Status.none
                }else if (material.emission.contents as! UIColor == UIColor.red) {
                    break
                }
                else {
                    material.emission.contents = UIColor.blue
                    status = Status.select
                }
                
            }
            SCNTransaction.commit()
        }
    }
    
    @objc
    func didDoubleTap(_ gesture: UITapGestureRecognizer) {
        let touchLocation = gesture.location(in: sceneView)
        
        let hitResults = sceneView.hitTest(touchLocation, options: [:])
        // check that we clicked on at least one object
        if hitResults.count > 0 {
            // retrieved the first clicked object
            let result = hitResults[0]
            
            // get its material
            let materials = result.node.geometry!.materials//.firstMaterial!
            // highlight it
            SCNTransaction.begin()
            for material in materials {
                if (material.emission.contents as! UIColor == UIColor.red) {
                    material.emission.contents = UIColor.black
                    status = Status.none
                }else {
                    material.emission.contents = UIColor.red
                    status = Status.trigger
                }
                
            }
            SCNTransaction.commit()
        }
    }
    
    @objc
    func didLongPress(_ gesture: UILongPressGestureRecognizer) {
        if status == Status.select {
            status = Status.none
        }else {
            status = Status.select
        }
    }
    
    func gestureRecognizer(_ gestureRecognizer: UIGestureRecognizer, shouldRecognizeSimultaneouslyWith otherGestureRecognizer: UIGestureRecognizer) -> Bool {
        // Allow objects to be translated and rotated at the same time.
        return true
    }

    /// A helper method to return the first object that is found under the provided `gesture`s touch locations.
    /// - Tag: TouchTesting
    private func objectInteracting(with gesture: UIGestureRecognizer, in view: ARSCNView) -> VirtualObject? {
        for index in 0..<gesture.numberOfTouches {
            let touchLocation = gesture.location(ofTouch: index, in: view)
            
            // Look for an object directly under the `touchLocation`.
            if let object = sceneView.virtualObject(at: touchLocation) {
                return object
            }
        }
        
        // As a last resort look for an object under the center of the touches.
        return sceneView.virtualObject(at: gesture.center(in: view))
    }
    
    // MARK: - Update object position

    /// - Tag: DragVirtualObject
    private func translate(_ object: VirtualObject, basedOn screenPos: CGPoint, infinitePlane: Bool) {
        guard let cameraTransform = sceneView.session.currentFrame?.camera.transform,
            let (position, _, isOnPlane) = sceneView.worldPosition(fromScreenPosition: screenPos,
                                                                   objectPosition: object.simdPosition,
                                                                   infinitePlane: infinitePlane) else { return }
        
        /*
         Plane hit test results are generally smooth. If we did *not* hit a plane,
         smooth the movement to prevent large jumps.
         */
        object.setPosition(position, relativeTo: cameraTransform, smoothMovement: !isOnPlane)
    }
}

/// Extends `UIGestureRecognizer` to provide the center point resulting from multiple touches.
extension UIGestureRecognizer {
    func center(in view: UIView) -> CGPoint {
        let first = CGRect(origin: location(ofTouch: 0, in: view), size: .zero)

        let touchBounds = (1..<numberOfTouches).reduce(first) { touchBounds, index in
            return touchBounds.union(CGRect(origin: location(ofTouch: index, in: view), size: .zero))
        }

        return CGPoint(x: touchBounds.midX, y: touchBounds.midY)
    }
}
