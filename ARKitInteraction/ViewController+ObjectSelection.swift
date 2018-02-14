/*
See LICENSE folder for this sample’s licensing information.

Abstract:
Methods on the main view controller for handling virtual object loading and movement
*/

import UIKit
import SceneKit

extension ViewController: VirtualObjectSelectionViewControllerDelegate {
    /**
     Adds the specified virtual object to the scene, placed using
     the focus square's estimate of the world-space position
     currently corresponding to the center of the screen.
     
     - Tag: PlaceVirtualObject
     */
    func placeVirtualObject(_ virtualObject: VirtualObject) {
        guard let cameraTransform = session.currentFrame?.camera.transform,
            let focusSquarePosition = focusSquare.lastPosition else {
            statusViewController.showMessage("CANNOT PLACE OBJECT\nTry moving left or right.")
            return
        }
        
        virtualObjectInteraction.selectedObject = virtualObject
        virtualObject.setPosition(focusSquarePosition, relativeTo: cameraTransform, smoothMovement: false)
        
        updateQueue.async {
            self.sceneView.scene.rootNode.addChildNode(virtualObject)
        
//            self.sceneView.scene.rootNode.childNode(withName: "rotateButton", recursively: true)?.isHidden = true
//            self.sceneView.scene.rootNode.childNode(withName: "rotateButtonPlane", recursively: true)?.isHidden = true
//            var val:Float = 0.0
//            for _ in 0..<10 {
//              //  self.highlightNode2(self.sceneView.scene.rootNode.childNode(withName: "buttonPlane", recursively: true)!, val: val)
//                //self.highlightNode2(self.sceneView.scene.rootNode.childNode(withName: "textBox", recursively: true)!, val: val)
//                val = val + 0.0001
//            }
//            let textGeometry = self.sceneView.scene.rootNode.childNode(withName: "infoText", recursively: true)
//            let text = textGeometry?.geometry as! SCNText
//            text.string = "Cornflakes \nHere we can Display all kinds of \ninformation like the nutrition or \nexpiration date and other stuff"
            
//            self.sceneView.scene.rootNode.childNode(withName: "buttonPlane", recursively: true)?.isHidden = true
//            self.sceneView.scene.rootNode.childNode(withName: "textBox", recursively: true)?.isHidden = true
        }
    }
    func createLineNode(fromPos origin: SCNVector3, toPos destination: SCNVector3, color: UIColor) -> SCNNode {
        let line = lineFrom(vector: origin, toVector: destination)
        let lineNode = SCNNode(geometry: line)
        let planeMaterial = SCNMaterial()
        planeMaterial.diffuse.contents = color
        line.materials = [planeMaterial]
        
        return lineNode
    }
    
    func lineFrom(vector vector1: SCNVector3, toVector vector2: SCNVector3) -> SCNGeometry {
        let indices: [Int32] = [0, 1]
        
        let source = SCNGeometrySource(vertices: [vector1, vector2])
        let element = SCNGeometryElement(indices: indices, primitiveType: .line)
        
        return SCNGeometry(sources: [source], elements: [element])
    }
    
    
    func highlightNode(_ node: SCNNode) {
        let (min, max) = node.boundingBox
        let zCoord = node.position.z
        let topLeft = SCNVector3Make(min.x, max.y, zCoord)
        let bottomLeft = SCNVector3Make(min.x, min.y, zCoord)
        let topRight = SCNVector3Make(max.x, max.y, zCoord)
        let bottomRight = SCNVector3Make(max.x, min.y, zCoord)
        
        let color = UIColor(red:0.00, green:0.59, blue:1.00, alpha:1.0)
        let bottomSide = createLineNode(fromPos: bottomLeft, toPos: bottomRight, color: color)
        let leftSide = createLineNode(fromPos: bottomLeft, toPos: topLeft, color: color)
        let rightSide = createLineNode(fromPos: bottomRight, toPos: topRight, color: color)
        let topSide = createLineNode(fromPos: topLeft, toPos: topRight, color: color)
        
        [bottomSide, leftSide, rightSide, topSide].forEach {
            $0.name = "highlightedNode"// Whatever name you want so you can unhighlight later if needed
            node.addChildNode($0)
        }
    }
    
    func highlightNode2(_ node: SCNNode, val: Float) { //Second method to make line thicker
        let (min, max) = node.boundingBox
        let zCoord = node.position.z
        let topLeft = SCNVector3Make(min.x - val, max.y + val, zCoord)
        let bottomLeft = SCNVector3Make(min.x - val, min.y - val, zCoord)
        let topRight = SCNVector3Make(max.x + val, max.y + val, zCoord)
        let bottomRight = SCNVector3Make(max.x + val, min.y - val, zCoord)
        
        let color = UIColor(red:0.00, green:0.59, blue:1.00, alpha:1.0)
        let bottomSide = createLineNode(fromPos: bottomLeft, toPos: bottomRight, color: color)
        let leftSide = createLineNode(fromPos: bottomLeft, toPos: topLeft, color: color)
        let rightSide = createLineNode(fromPos: bottomRight, toPos: topRight, color: color)
        let topSide = createLineNode(fromPos: topLeft, toPos: topRight, color: color)
        
        [bottomSide, leftSide, rightSide, topSide].forEach {
            $0.name = "highlightedNode"// Whatever name you want so you can unhighlight later if needed
            node.addChildNode($0)
        }
    }
    
    // MARK: - VirtualObjectSelectionViewControllerDelegate
    
    func virtualObjectSelectionViewController(_: VirtualObjectSelectionViewController, didSelectObject object: VirtualObject) {
        virtualObjectLoader.loadVirtualObject(object, loadedHandler: { [unowned self] loadedObject in
            DispatchQueue.main.async {
                self.hideObjectLoadingUI()
                self.placeVirtualObject(loadedObject)
            }
        })

        displayObjectLoadingUI()
    }
    
    func virtualObjectSelectionViewController(_: VirtualObjectSelectionViewController, didDeselectObject object: VirtualObject) {
        guard let objectIndex = virtualObjectLoader.loadedObjects.index(of: object) else {
            fatalError("Programmer error: Failed to lookup virtual object in scene.")
        }
        virtualObjectLoader.removeVirtualObject(at: objectIndex)
    }

    // MARK: Object Loading UI

    func displayObjectLoadingUI() {
        // Show progress indicator.
        spinner.startAnimating()
        
        addObjectButton.setImage(#imageLiteral(resourceName: "buttonring"), for: [])

        addObjectButton.isEnabled = false
        isRestartAvailable = false
    }

    func hideObjectLoadingUI() {
        // Hide progress indicator.
        spinner.stopAnimating()

        addObjectButton.setImage(#imageLiteral(resourceName: "add"), for: [])
        addObjectButton.setImage(#imageLiteral(resourceName: "addPressed"), for: [.highlighted])

        addObjectButton.isEnabled = true
        isRestartAvailable = true
    }
}
