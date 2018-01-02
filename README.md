
---- Projekt nutzt GitLFS, das muss vorher installiert werden 
---- Link: https://git-lfs.github.com/  


Readme.md
.gitignore 
.gitattributes hab ich schonnmal eingestellt. Sollte mehr zur einer der beiden Listen kommen, 
	sollte das in der Masterbranch passieren, damit jeder auf dem selben Stand bleibt. 
		(Das einfach Absprechen)

		
Arbeitshinweise:
		
1.Jede Sub-Gruppe sollte immer auf ihrer eigenen Branch arbeiten

2.UNBEDINGT für die Scripts Namespaces nutzen bsp.: 
	using SAP.General;
	
	namespace SAP.ARKit 
	{ 
		public Klassenname 
		{ 
			//Code
		}
	}
	
3.Nach Möglichkeit nicht ohne Absprache in den Project Settings sachen einstellen (
	-> vllt sollte das generell in der Masterbranch dann geschehen, damit es später beim mergen nicht zuviel durcheinander gibt
		Selbes Prinzip wie oben bei der ignore datei

		
		
Ich habe die Ordnerstruktur als Vorschlag vorgegeben. Wäre für die Ordnung ganz praktisch, 
wenn wir das nach Möglichkeit so beibehalten, wenn was fehlt einfacht adden bei euch.

Die Root Ordnerstruktur sollte UNBEDINGT beibehalten werden in den Branches. Jede Gruppe füllt Ihre sachen entweder in den Entsrpechenden Ordner (da hab ich auch ne struktur vorgeschlagen, 
wenn was fehlt einfach adden, wenn es auch garnicht passt könnt ihr es in eurem Ordner auch anders strukturieren)
ODER in den "general"-Ordner (wenn es für alle nützlich ist)

Third Party Sachen (vom Asset Store) können wir einfach so in den Root packen, so wie sie importiert werden.

Die Ordner für die Gruppen sind alle nummeriert, damit sie stets oben im Projektfenster innerhalb von Unity angezeigt werden.



--- Ich selbst benutz TortoiseGit ( https://tortoisegit.org/ ) 
--- für meine Git projekte, für Leute, die nicht soviel Lust haben mit GitBash zu hantieren : )