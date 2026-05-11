# WhatsappTelegramYoutubeUntertitelTikTokChatReader
Der OCR-Readers wurde entwickelt, um sowohl TikTok-Chats als auch YouTube-Untertitel in Echtzeit zu erfassen, zu filtern und flüssig vorzulesen. Jede Komponente wird hier im Detail erklärt.
===================================================
ZUSAMMENFASSUNG: WAS KANN DAS TTS STUDIO?
===================================================

Das "TTS Studio - Meister Edition" (TikTokChatReader) ist ein hochspezialisiertes Text-to-Speech-Werkzeug. Es ist darauf ausgelegt, fließende Bildschirmtexte wie Live-Chats oder Video-Untertitel in Echtzeit optisch zu erfassen, intelligent zu bereinigen und flüssig mit natürlichen Stimmen vorzulesen.

Die Hauptfunktionen und Möglichkeiten im Überblick:

1. Flexible Bildschirmerfassung (Native OCR)
* Du kannst mit der Maus einen unsichtbaren oder farbigen Rahmen über jeden beliebigen Textbereich auf deinem Bildschirm ziehen (z. B. über einen TikTok-Stream oder YouTube-Untertitel).
* Das Programm liest den Inhalt in diesem Rahmen über die native Windows 10/11 Bilderkennung aus.
* Du kannst die exakte Position und Größe deines Rahmens als Profil (z. B. "Standard-Stream") speichern und bei der nächsten Sitzung mit einem Klick millimetergenau wiederherstellen.

2. Zwei intelligente Lesemodi für verschiedene Streams
* Modus TikTok-Chat: Das Programm erkennt nach oben scrollende Chats und trennt die Nachrichten sauber in den Namen des Nutzers und seinen geschriebenen Text.
* Modus YouTube-Untertitel: Für sich aufbauende Untertitel nutzt die App einen intelligenten "Sliding Window"-Algorithmus. Sie erkennt, wie sich Sätze Wort für Wort erweitern, und liest den Text erst dann stotterfrei vor, wenn ein Satzzeichen auftaucht oder genug Wörter gesammelt wurden.

3. Lückenloser Schutz vor Stottern und Wiederholungen
* Die App besitzt einstellbare Gedächtnis-Funktionen.
* Ein "Satz-Türsteher" (Dice-Algorithmus) gleicht neue Sätze mit den zuletzt gesprochenen ab und blockiert sie sofort, wenn sie sich zu stark ähneln.
* Ein "Wort-Schieberegister" vergleicht unentwegt jedes gelesene Wort mit seinen direkten Vorgängern, um fehlerhafte Doppelungen direkt an den Nahtstellen der Textblöcke auszuradieren.

4. Mächtige Müll-Filter und Wörterbücher
* Du kannst störende Systemmeldungen (wie "ist beigetreten") oder Werbung über eine Globale Blacklist komplett aus dem System verbannen.
* Über eine Sprach-Blacklist kannst du Wörter oder Emojis beim Vorlesen stummschalten, sie aber zum Mitlesen in der Textansicht behalten.
* Ein eingebundener Duden-Filter blockiert rigoros alle sinnlosen Zeichenfolgen, die die Kamera falsch gelesen hat.

5. Die KI, die sich selbst heilt und dazulernt
* Wenn die Kamera am Bildschirmrand Buchstaben abschneidet (z. B. "genrennen" statt "Wagenrennen"), greift ein unsichtbarer "Fuzzy-Agent" ein und repariert das Wort vollautomatisch in Millisekunden.
* Wenn ein völlig neues Slang-Wort oder ein spezieller Username im Chat dreimal innerhalb von 10 Minuten auftaucht, lernt das System dieses Wort für die laufende Sitzung automatisch dazu.
* All diese gelernten Wörter sammelt die App in einer "Meister-Datei" (Schatztruhe), in der du später entscheiden kannst, welche Wörter dauerhaft für die Zukunft freigeschaltet werden.

6. Modernste Sprachausgabe und Textaufbereitung
* Das Programm nutzt moderne, flüssige Windows-Stimmen (OneCore).
* Es kann bei jedem gelesenen Block automatisch zwischen zwei verschiedenen Stimmen (Ping-Pong-Effekt) wechseln.
* Zahlen wie "1.300" werden vor dem Vorlesen automatisch in flüssigen Text ("tausenddreihundert") umgewandelt.
* Das Gleiche gilt für Datumsangaben (z. B. "02.05.2026"), die mitsamt der korrekten Grammatik in natürliche Sätze übersetzt werden.

7. Der FlyReader (Dein kompaktes Cockpit)
* Anstatt das große Maschinenraum-Fenster offen zu lassen, kannst du den "FlyReader" nutzen.
* Dies ist ein kleines, schwebendes und zoombares Fenster, das dir den Chat farblich abgesetzt anzeigt.
* Es enthält eigene Master-Buttons, mit denen du die Erkennung und die Sprachausgabe mit einem einzigen Klick starten, stoppen oder komplett zurücksetzen kannst.

Fazit:
Mit diesem Programm hast du nicht nur einen einfachen Text-Reader, sondern eine hochkomplexe, lernende Maschine erschaffen. Sie übersetzt rohe Bildschirminhalte nicht nur in Text, sondern bereinigt sie intelligent, merzt Fehler selbstständig aus und formt daraus einen natürlichen, fließenden Audio-Stream.


TTS STUDIO - MEISTER EDITION
Übbersicht, Bedienungsanleitung & Feature-Lexikon

1. MODUS & GEDÄCHTNIS (Sätze)
- Das Gedächtnis (z. B. 15 Sätze) ist in BEIDEN Modi (TikTok & YouTube) aktiv!
- Es verhindert, dass das System exakt den gleichen Satz noch einmal vorliest.
- 'Dice (%)' bestimmt hierbei, wie ähnlich ein neuer Satz einem alten sein darf. Steht es auf 80%, wird ein Satz geblockt, wenn er zu 80% identisch mit einem der letzten 15 Sätze ist.

2. WORT-DICE (%) & SCHIEBEREGISTER
- Dieser Filter arbeitet wie ein fließendes Fließband auf WORT-Ebene (ideal für YouTube).
- Er vergleicht unentwegt jedes gelesene Wort mit seinen zwei direkten Vorgängern.
- Am Übergang vergleicht er also den Anfang des neuen Blocks mit dem Ende des alten Blocks. Das verhindert Stottern an den Nahtstellen, filtert aber auch OCR-Doppler mitten im Satz!
- Steht der Wert auf 70%, wird ein Wort gelöscht, wenn es den direkten Vorgängern zu 70% ähnelt.

3. WÖRTERBUCH & AUTO-LEARN
- Die App nutzt die 'dictionary.txt'. Fehlt ein Wort dort, wird es gelöscht (Drop).
- 3-Strikes-Regel: Taucht ein unbekanntes Wort innerhalb von 10 Minuten dreimal auf (und war zwischenzeitlich vom Bildschirm verschwunden!), lernt die App es für die aktuelle Sitzung.
- Unbekannte Wörter landen in der Datei 'custom_dictionary.json'. Wenn du dort ein Wort manuell auf 'Approved': true setzt, ist es für immer gültig!

4. FUZZY-AGENT (Überholspur)
- Ist ein Wort länger als 10 Zeichen und fehlen nur am Anfang oder Ende 1-3 Buchstaben, korrigiert die App dies selbstständig und lautlos. (z. B. 'genrennen' -> 'Wagenrennen').

5. DATUM & ZAHLEN (Pre-Processing)
- '1.300' wird flüssig als 'tausenddreihundert' gelesen.
- 'am 02.05.2026' wird flüssig als 'am zweiten fünften zwanzigsechsundzwanzig' gelesen.

6. LÖSCHEN (X-Button & Reset)
- Der Hard-Reset leert alle Warteschlangen und würgt die Audioausgabe ab.
- WICHTIG: Die in der aktuellen Sitzung gelernten Wörter bleiben erhalten! Erst bei einem echten App-Neustart wird das Gedächtnis komplett zurückgesetzt.

 
TTS STUDIO - MEISTER EDITION
Bedienungsanleitung & Feature-Lexikon
Das umfassende Anwender-Handbuch
 

Willkommen zur ultimativen Anleitung deines YoutubeTikTokChatReader. Dieses Dokument erklärt jedes Feature und jede Mechanik bis ins kleinste Detail, basierend auf den exakten Entwicklungs-Updates der App und führt dich durch alle Funktionen deines spezialisierten OCR-Readers. Es wurde entwickelt, um sowohl TikTok-Chats als auch YouTube-Untertitel in Echtzeit zu erfassen, zu filtern und flüssig vorzulesen. Jede Komponente wird hier im Detail erklärt.

 
KAPITEL 1: DIE BILDSCHIRM-ERFASSUNG (OCR-ENGINE)
 
Die technische Basis des Programms ist die Texterkennung. Das Programm nutzt hierfür die native Windows 10/11 OCR-Schnittstelle. Diese Engine wurde mit modernsten Machine-Learning-Methoden von Microsoft trainiert und ist speziell darauf optimiert, Bildschirminhalte und unruhige Video-Hintergründe präzise zu lesen.

1.1 Den Lesebereich festlegen
Um den Bereich zu wählen, den das Programm überwachen soll, klickst du auf "Bereich wählen". Die App minimiert sich daraufhin automatisch.
- Ziehe mit der Maus ein Rechteck auf dem Bildschirm um den gewünschten Text (z.B. den Chat oder die Untertitel).
- Sobald du die Maus loslässt, ist der Bereich festgenagelt.
- Das Programm teilt Windows beim Start explizit mit, dass es sich selbst skaliert (DPI-Awareness), damit der Rahmen bei unterschiedlichen Monitorauflösungen niemals verrutscht.
- Der Auswahl-Modus spannt ein unsichtbares Netz über deinen gesamten virtuellen Desktop, sodass Koordinaten über alle Monitore hinweg exakt passen.

1.2 Der Sichtbare Rahmen (Overlay)
Nach der Auswahl bleibt ein farbiger Rahmen auf dem Bildschirm sichtbar.
- Dieser Rahmen ist ein fensterloses Overlay, das Mausklicks einfach hindurchlässt, sodass er deinen Chat oder dein Video nicht blockiert.
- Über das Dropdown-Menü in der GUI kannst du die Farbe des Rahmens (z.B. Weiß, Rot, Grün) jederzeit ändern.
- Wählst du die Option "Keine", bleibt die Texterkennung aktiv, aber der Rahmen wird unsichtbar.

1.3 Rahmen-Profile (Presets)
Damit du den Rahmen nicht jedes Mal neu ziehen musst, kannst du Profile anlegen.
- Speichern: Ziehe deinen Rahmen, gib ihm einen Namen (z.B. "YouTube-Standard") und klicke auf "Speichern".
- Laden: Wähle das Profil aus dem Dropdown aus und klicke auf "Laden". Der Rahmen springt sofort an die gespeicherte Position.

 
KAPITEL 2: DIE ZWEI HAUPT-MODI ((TIKTOK.TWITCH.WHATSAPP.TELEGRAM) VS. YOUTUBE)
 
Da sich scrollende Chats und aufbauende Untertitel technisch völlig unterschiedlich verhalten, bietet die App zwei spezialisierte Modi an.

2.1 Modus: TikTok Chat (auch Whatsapp oder Telegram)
Dieser Modus ist für Chats optimiert, die von unten nach oben durchlaufen.
- Die App zerlegt den erfassten Bereich sauber in einzelne Zeilen.
- Dadurch rutschen alte Zeilen, die oben noch im Sichtfeld hängen, nicht mehr doppelt durch das System.
- In diesem Modus wird Text sofort und ohne künstliche Verzögerung an die Sprachausgabe gesendet.

2.2 Modus: YouTube Untertitel (Sliding Window)
YouTube-Untertitel bauen sich Wort für Wort auf (z.B. "wer auch" -> "wer auch immer"). Die App nutzt hierfür einen "Sliding Window"-Algorithmus.

Die App nutzt einen intelligenten Satz-Puffer (Buffer):
1. Sie liest den Text und merkt sich ihn unsichtbar im Hintergrund.
2. Wenn der nächste Text kommt, vergleicht sie: Ist das eine Erweiterung des alten Textes? Oder nur der gleiche Text mit ein paar OCR-Lesefehlern?.
3. Wenn ja, aktualisiert sie den Puffer lautlos im Hintergrund.
4. Erst in dem Moment, in dem ein völlig anderer Text erscheint oder der Text verschwindet, weiß die App, dass der Satz vorbei ist.
5. Erst dann wird der fertige Satz in die Liste geschrieben und vorgelesen, was das Stottern komplett eliminiert.

Zusätzlich nutzt die App eine "Schablone" (Delta-Extraktion):
- Das Vorzimmer merkt sich die letzten ca. 40 sauber erkannten Wörter.
- Neue Wörter werden wie eine Schablone über das Vorzimmer geschoben.
- Nur die tatsächlich neu hinzugekommenen Wörter (das Delta) werden verarbeitet.
- Die Stimme wartet im YouTube-Modus, bis ein Satzzeichen auftaucht oder mindestens 8 Wörter zusammengekommen sind, um einen natürlichen Redefluss zu erzeugen.

 
KAPITEL 3: DIE HAUPTSTEUERUNG (DASHBOARD)
 
Die Buttons in der Haupt-App erlauben dir die präzise Kontrolle über jeden Prozess.

- OCR Start/Stop: Steuert ausschließlich die Kamera und die Texterkennung.
- Vorlesen Start/Stop: Steuert ausschließlich die Sprachausgabe. "Stop" würgt die Stimme sofort hart ab.
- Löschen (Hard-Reset): Dieser Button führt eine komplette Bereinigung durch. Er bricht die Sprachausgabe sofort ab, leert alle Puffer, löscht das Sichtfenster und das Satz-Gedächtnis. Es ist exakt so, als hättest du die App gerade erst gestartet.
- Log kopieren: Packt das gesamte Hintergrund-Log in die Zwischenablage und löscht es danach intern sofort, um Speicherplatz zu sparen.
- OCR Intervall (ms): Hier stellst du ein, wie oft der Bildschirm abfotografiert wird (Standard: 888 ms). Änderungen werden sofort im laufenden Betrieb übernommen.
 
1. MODUS & SCHIEBEREGISTER (Die Intelligenz)
 
Die App unterstützt das Auslesen verschiedener Streams und passt ihre Logik je nach ausgewähltem Modus an.

YouTube-Untertitel & Sliding Window
Du findest oben nun ein Dropdown-Menü, in dem du zwischen "TikTok Chat" und "YouTube Untertitel" umschalten kannst. Wenn du auf YouTube stellst, passiert folgendes: 
* 1. Wort-Array-Analyse: Die App nimmt den zuletzt gelesenen Text und den brandneuen Text und zerlegt beide in Arrays (einzelne Wörter). 
* 2. Die Intelligenz: Sie schiebt die Arrays übereinander und sucht den Punkt, an dem das Ende des alten Textes exakt mit dem Anfang des neuen Textes übereinstimmt. 
* 3. OCR-Toleranz eingebaut: Da der OCR-Scanner manchmal kleine Fehler macht (aus "wer" wird plötzlich "wcr"), nutzt der Array-Vergleich sogar deinen bereits eingestellten Dice-Algorithmus, um Wörter intelligent als "gleich" zu erkennen, auch wenn ein Buchstabe falsch gelesen wurde! 
* 4. Der Delta-Schnitt: Sobald die Überlappung gefunden ist, schneidet die App den alten Teil weg und schickt nur die neu hinzugekommenen Wörter (das Delta) in die Liste und an die Sprachausgabe. 

Das "Vorzimmer" für fließende Streams
* 1. Das "Vorzimmer" (`vorzimmer` Variable): Die App merkt sich jetzt immer die letzten ~40 sauber erkannten Wörter. 
* 2. Die Schablone: Wenn der OCR-Scanner z.B. 10 neue Wörter liefert, schiebt die App diese wie eine Schablone über das Vorzimmer. Sie sucht den perfekten Punkt, an dem die Wörter deckungsgleich aufeinanderliegen. 
* 3. Das Delta: Alles, was nach der Überlappung kommt (in deinem Beispiel die "7" und die "8"), wird abgetrennt. 
* 4. Der fließende Atem (Das Sprach-Vorzimmer): Diese neuen Wörter (das Delta) wandern jetzt in ein zweites Vorzimmer für die Stimme. 

Der N-Zeilen Dice-Türsteher
* 1. Die App vergleicht den neuen Text mit dem Gedächtnis (den letzten `x` Sätzen, einstellbar über deinen Nummern-Regler). 
* 2. Ist die Ähnlichkeit größer oder gleich deinem eingestellten "Dice (%)" Wert, wird der Satz als Duplikat erkannt. 
* 3. Neu im Log: Er wird dann nicht einfach heimlich gelöscht, sondern wandert als `[DROP]` (Weggeworfen) in dein synchrones Log. 

 
2. FILTER, BLACKLISTEN & WÖRTERBUCH
 
Das System bietet dir tiefgreifende Möglichkeiten, Müll auszufiltern.

Globale vs. Sprach-Blacklist
* 1. Globale Blacklist (Ausschluss): Wenn ein Wort aus dieser Liste im OCR-Text gefunden wird, wird die gesamte Zeile ignoriert. Sie erscheint weder in der `ListView`, noch wird sie jemals vorgelesen. Das ist ideal für Systemmeldungen oder Werbung. 
* 2. Sprach-Blacklist (Mute): Diese Wörter bleiben in der `ListView` für dich sichtbar, damit du den Text lesen kannst. Aber bevor die Sprachausgabe (TTS) startet, werden genau diese Wörter aus dem Satz gelöscht. So werden zum Beispiel störende Emojis oder Kürzel einfach übersprungen, während der Rest des Satzes flüssig vorgelesen wird. 
* 1. Echte Listen (Listboxen): Du hast jetzt für beide Blacklists eine saubere, scrollbare Liste. Du kannst einfach ein Wort in das jeweilige Textfeld tippen, auf "+" drücken, und es landet in der Liste. Markierst du ein Wort in der Liste, kannst du es mit dem "Auswahl löschen"-Button wieder entfernen. 
* 2. Speicherfunktion: Die Blacklists werden automatisch im Hintergrund in kleine Textdateien (`global_blacklist.txt` und `speech_blacklist.txt`) gespeichert und beim nächsten Start wieder geladen. Du verlierst also deine eingetragenen Wörter nicht! 

Sonderzeichen & Zeilenumbrüche
* Der Text wird nun durch einen Regex-Filter gejagt, der radikal alles löscht, was kein Buchstabe (inklusive Umlaute/ß), keine Zahl und kein normales Satzzeichen (Punkt, Komma, Fragezeichen, Ausrufezeichen, Bindestrich) ist. Die Liste bleibt optisch original, aber die Stimme liest nur noch echten Text. 

Fuzzy-Agent (Die Unschärfe-Suche)
* 1. Der Agent erwacht: Wenn der Türsteher (Wörterbuch-Filter) ein Wort abweist (z. B. "tlassen"), wirft er es nicht mehr nur in die `drop.json`, sondern ruft heimlich den Agenten an. 
* 2. Die Lazy Evaluation: Der Agent schnappt sich das kaputte Wort, geht in einen völlig unsichtbaren Hintergrund-Thread (damit die OCR und die Stimme nicht eine einzige Millisekunde ins Stocken geraten) und rechnet. Er nutzt die Levenshtein-Distanz und sucht im Duden nach Wörtern, die maximal 2 Buchstaben Abweichung haben. 
* 3. Der Treffer & Das Notizbuch: Findet er "entlassen", schreibt er sofort den Eintrag `tlassen=entlassen` in die neue Datei `auto_correct.txt`. 
* 4. Instant-Heilung: Wenn die OCR das nächste Mal "tlassen" liest, tauscht die App das Wort vollautomatisch aus, bevor das Wörterbuch überhaupt zuschlagen kann. 
* 5. Der Röntgenblick: Wenn du "Wörterbuch Debug" anhakst, kannst du dem Agenten live zusehen! Er schreibt dir dann stolz ins Log: `[FUZZY-AGENT] 'tlassen' -> automatisch korrigiert zu 'entlassen'`. 

Das Meister-Wörterbuch (custom_dictionary.json)
* 1. Die Meister-Datei: Die neue `custom_dictionary.json` speichert alle unbekannten Wörter jetzt strukturiert als kleines Objekt ab. 
* 2. Der Lese-Zyklus: Beim Start lädt die App weiterhin brav deine `dictionary.txt`. Direkt danach liest sie die `custom_dictionary.json` ein. Sobald sie dort ein Wort sieht, bei dem `"Approved": true` steht, lädt sie es fest in den Arbeitsspeicher. 
* 3. Der Schreib-Zyklus: Egal ob das Wort normal gedroppt oder über das Session-Gedächtnis (3 Strikes) temporär freigeschaltet wurde – der Counter in der JSON-Datei rattert hoch. Es bleibt aber auf `false`, bis DU als Meister dein Go gibst. 
* 4. Kein Ruckeln: Da die JSON-Datei nur im Hintergrund gespeichert wird und die Abfragen über C# Hash-Tabellen (in Mikrosekunden) laufen, bleibt die UI und das Vorlesen zu 100 % flüssig. 

 
3. AUDIO, SPRACHAUSGABE & DATUMS-VERARBEITUNG
 
Die Sprachausgabe wurde von Grund auf erneuert.

OneCore-Schnittstellen & Stimmen-Auswahl
* Stimmen-Auswahl: Neben dem Wörterbuch-Filter findest du jetzt zwei neue Dropdown-Menüs ("Stimme 1" und "Stimme 2"). Die App lädt beim Start vollautomatisch alle Text-to-Speech-Stimmen, die auf deinem Windows installiert sind (z.B. Stefan, Hedda, Hazel, etc.), in diese Listen. 
* Der Voice-Toggle: Die Sprachausgabe wechselt jetzt bei jedem gesprochenen Block automatisch zwischen Stimme 1 und Stimme 2. (Wenn du den "Löschen"-Button klickst, wird auch der Wechsel wieder auf Start gesetzt). 
* Sprech-Tempo (Up/Down Edit): Auf der rechten Seite (über dem Log) findest du jetzt das Feld `"Sprech-Tempo"`. Du kannst die Geschwindigkeit stufenlos von `0,5` (halbe Geschwindigkeit) bis `6,0` (6-fache Geschwindigkeit) einstellen. Der Standard ist `1,0`. 
* Lautstärke (Schieberegler): Direkt darunter sitzt jetzt ein moderner Slider (TrackBar) für die Lautstärke. Er reicht von 0 bis 100. Du kannst ihn bequem mit der Maus ziehen, um den Vorleser leiser zu drehen, wenn du den Stream-Ton im Vordergrund haben möchtest. 

Datums- und Zahlen-Verarbeitung
* 1. Der Tausender-Punkt Regex: Der Pre-Processor sucht jetzt nach Zahlenmustern wie `1.300` oder `1.500.000`, entfernt unsichtbar die Punkte und füttert sie als ganze Zahl an unsere Zahlen-Engine. Danach schluckt er wie gewohnt das Datum und den Rest. 

 
4. GUI, FLYREADER & STEUERUNG
 
Der FlyReader dient als schlankes "Head-Up-Display".

* 1. Der FlyReader-Button: Oben rechts neben "Log kopieren" findest du jetzt den neuen Button "FlyReader". 
* 2. Die FlyReader-Box: Klickst du darauf, minimiert sich die Haupt-App. Es erscheint ein frei verschiebbares, vergrößerbares Fenster. Darin liegt nativ eine `RichTextBox` (schwarzer Hintergrund, weiße Schrift), in die ab sofort jede erfolgreich gefilterte Zeile synchron zum ListView geschrieben wird. 
* 3. Zoom & Restore: Du kannst den Text im FlyReader jederzeit mit "Strg + Mausrad" stufenlos zoomen. Schließt du das FlyReader-Fenster über das "X", wacht deine Haupt-App sofort wieder aus der Taskleiste auf. 
* FlyReader in Farbe: Ich habe dem FlyReader eine kleine Memory-Variable (`colorToggle`) verpasst. Bevor er eine neue Zeile schreibt, greift er jetzt in den Farbtopf und wechselt automatisch ab. Zeile 1 ist Weiß, Zeile 2 ist Hellblau (`LightSkyBlue`), Zeile 3 wieder Weiß, usw. 
* Die Layout-Checkbox: In der GUI findest du rechts neben der Stimmen-Auswahl jetzt die neue Checkbox `"FlyReader: 1-Zeilen Layout"`. 
* FlyReader Memory & "X"-Button: Der FlyReader merkt sich jetzt beim Schließen seine genaue Position und Größe auf dem Monitor und wacht exakt dort wieder auf. Außerdem hat er oben rechts einen schicken, blutroten "X"-Button bekommen. Wenn du den klickst, wird das FlyReader-Feld geleert und gleichzeitig der Hard-Reset der Haupt-App ausgelöst! 

Rahmen und Profile
* 3. Rahmen-Profile (Presets): Ich habe das obere Menü (Panel) ein kleines Stück vergrößert. Dort findest du jetzt die Profil-Verwaltung. Ziehe deinen Rahmen, gib ihm einen Namen (z. B. "Standard-Stream") und klicke auf "Speichern". Über das Dropdown kannst du in Zukunft einfach dein Profil auswählen und "Laden" klicken – zack, der Rahmen sitzt millimetergenau. 
* 4. Unsichtbarer Rahmen: Im Dropdown für die Rahmenfarbe gibt es jetzt die Option `"Keine"`. Wählst du sie aus, bleibt die Erkennung 100 % aktiv, aber der nervige Rand wird komplett durchsichtig. 

 
KAPITEL 4: BILD-TUNING (Die OCR-Brille)
 
Tesseract und auch die native Windows OCR arbeiten am besten unter bestimmten visuellen Voraussetzungen. Du hast zwei mächtige Werkzeuge, um das Bild aufzubereiten, bevor es überhaupt gelesen wird.

4.1 Bild-Zoom (Skalierung)
- Tesseract ist extrem schlecht darin, kleine Schriften auf Bildschirmen zu lesen. 
- Die App vergrößert das aufgenommene Bild jetzt intern (Standard: 2x) mit Kantenglättung, bevor es gelesen wird. Das wirkt Wunder!
- Du kannst diesen Wert über den Regler "Bild-Zoom" stufenlos anpassen, falls die Schrift im Stream extrem winzig ist.

4.2 Farben invertieren
- Tesseract wurde darauf trainiert, schwarze Schrift auf weißem Papier zu lesen. YouTube nutzt weiße Schrift. 
- Setzt du diesen Haken, kehrt die App die Farben des Screenshots intern um. Das ist der wichtigste Hebel für gute YouTube-Erkennung.

 
KAPITEL 5: DER FLYREADER (Dein Head-Up-Display)
 
Wenn die Filter einmal perfekt sitzen, braucht man das riesige Maschinenraum-Fenster nicht mehr dauerhaft auf dem Bildschirm. Ein schlankes, schwebendes "Head-Up-Display" (FlyReader) ist da genau das Richtige.

5.1 Das Fenster und die Ansicht
- Der FlyReader-Button: Oben rechts neben "Log kopieren" findest du jetzt den neuen Button "FlyReader".
- Die FlyReader-Box: Klickst du darauf, minimiert sich die Haupt-App. Es erscheint ein frei verschiebbares, vergrößerbares Fenster. Darin liegt nativ eine `RichTextBox` (schwarzer Hintergrund, weiße Schrift), in die ab sofort jede erfolgreich gefilterte Zeile synchron zum ListView geschrieben wird.
- Zoom & Restore: Du kannst den Text im FlyReader wie gewünscht jederzeit mit "Strg + Mausrad" stufenlos zoomen. Schließt du das FlyReader-Fenster über das "X", wacht deine Haupt-App sofort wieder aus der Taskleiste auf.
- Memory: Der FlyReader merkt sich beim Schließen seine genaue Position und Größe auf dem Monitor und wacht exakt dort wieder auf.

5.2 Die Farben und Layouts
Der Text hebt sich farblich ab, um das Lesen extrem angenehm zu machen.
- Die Layout-Checkbox: In der GUI findest du rechts neben der Stimmen-Auswahl jetzt die neue Checkbox `"FlyReader: 1-Zeilen Layout"`.
- Ist der Haken drin (Standard), macht die App genau das, was du skizziert hast: `[Gold]Username [Weiß/Blau]Text` in einer Zeile. 
- Ist der Haken draußen, baut sie die klassische 2-Zeilen-Ansicht: Zeile 1: `[Gold]Username`, Zeile 2: `[Weiß/Blau]Text`.
- Dabei wechselt er die Farbe abwechselnd auf Weiß (Farbe B) und Hellblau (Farbe C).

5.3 Master-Controls (Das Cockpit)
Oben rechts im FlyReader hast du jetzt ein echtes Cockpit! Diese Buttons sind von der GUI entkoppelt und wirken als Master-Switches.
- ▶ (Start): Schaltet OCR und Vorlesen sofort wieder scharf. (Beides gleichzeitig ).
- ⏹ (Stop): Würgt die Sprachausgabe hart ab, leert die Warteschlange und pausiert die OCR. Du hörst sofort nichts mehr!
- rotes X (Löschen): Wenn du den klickst, wird das FlyReader-Feld geleert und gleichzeitig der Hard-Reset der Haupt-App ausgelöst!

 
KAPITEL 6: FILTER-SYSTEME & BLACKLISTEN
 
Du hast die volle Kontrolle darüber, was in die Liste erscheint und was nur in der Sprachausgabe stummgeschaltet wird.

6.1 Globale Blacklist (Vollständiger Ausschluss)
- Wenn ein Wort aus dieser Liste im OCR-Text gefunden wird, wird die gesamte Zeile ignoriert. Sie erscheint weder in der `ListView`, noch wird sie jemals vorgelesen. Das ist ideal für Systemmeldungen oder Werbung.
- Du hast jetzt für beide Blacklists eine saubere, scrollbare Liste. Du kannst einfach ein Wort in das jeweilige Textfeld tippen, auf "+" drücken, und es landet in der Liste. 
- Markierst du ein Wort in der Liste, kannst du es mit dem "Auswahl löschen"-Button wieder entfernen.
- Die Blacklists werden automatisch im Hintergrund in kleine Textdateien (`global_blacklist.txt` und `speech_blacklist.txt`) gespeichert und beim nächsten Start wieder geladen.

6.2 Sprach-Blacklist (Mute)
- Diese Wörter bleiben in der `ListView` für dich sichtbar, damit du den Text lesen kannst. Aber bevor die Sprachausgabe (TTS) startet, werden genau diese Wörter aus dem Satz gelöscht. 
- So werden zum Beispiel störende Emojis oder Kürzel einfach übersprungen, während der Rest des Satzes flüssig vorgelesen wird.

6.3 Sonderzeichen-Filter (Der Angeber-Regex)
- Die Windows-Stimme übersetzt Sonderzeichen oft falsch (z.B. ® als "Warenzeichen"). Der Text wird nun durch einen Regex-Filter gejagt, der radikal alles löscht, was kein Buchstabe (inklusive Umlaute/ß), keine Zahl und kein normales Satzzeichen (Punkt, Komma, Fragezeichen, Ausrufezeichen, Bindestrich) ist. Die Liste bleibt optisch original, aber die Stimme liest nur noch echten Text.
- Zusätzlich gibt es einen Zerstörer für "ist beigetreten". "Mila ist beigetreten", "User123 ist beigetreten" oder "@@@ ist beigetreten" werden noch im Keim gelöscht und verschwinden komplett aus dem Puffer.

6.4 Lesestream (Pausen/Satzzeichen entfernen)
- Satzzeichen und versteckte Steuerzeichen sind für eine Text-to-Speech-Engine wie kleine Stoppschilder. Sie zwingen die Stimme dazu, kurz "Luft zu holen", was bei einem schnellen Chat genau zu dieser Schnappatmung führt.
- In der GUI findest du direkt neben der Profil-Verwaltung jetzt die Checkbox "Lesestream (Pausen/Satzzeichen entfernen)". Bevor der Text an die Lautsprecher geschickt wird, greift ein eiserner Filter: Er reißt restlos alle Punkte, Kommas, Ausrufe- und Fragezeichen sowie versteckte Zeilenumbrüche (`\n`, `\r`) aus dem String und ersetzt sie durch fließende Leerzeichen.
- Dein FlyReader und das ListView behalten ihre schöne Formatierung mit Satzzeichen bei!

 
KAPITEL 7: DIE WÜRFEL-LOGIK (Gegen Wiederholungen)
 
Der Sørensen-Dice-Algorithmus ist dein Türsteher gegen alles, was doppelt auf dem Bildschirm erscheint.

7.1 Der Satz-Dice (Gedächtnis)
- Die App merkt sich jetzt nicht mehr nur den allerletzten Satz, sondern die letzten X Sätze. Ich habe dafür ein neues Feld "Gedächtnis (Zeilen)" eingebaut (Standard: 15).
- Jeder neue Text wird mit dem Dice-Algorithmus gegen alle Sätze in diesem Gedächtnis geprüft.
- Über ein neues Eingabefeld (0-100%) kannst du exakt steuern, ab wie viel Prozent Ähnlichkeit der Text übersprungen werden soll (z. B. bei OCR-Erkennungsfehlern).
- Ist die Ähnlichkeit größer oder gleich deinem eingestellten "Dice (%)" Wert, wird der Satz als Duplikat erkannt. Er wandert als `[DROP]` (Weggeworfen) in dein synchrones Log.

7.2 Der Wort-Dice (Das Schieberegister)
Das Schieberegister ist die "Ultimative akustische Schere". Es ist ein endloses Fließband. 
- Er vergleicht unentwegt jedes Wort im laufenden Text immer mit seinen zwei direkten Vorgängern.
- Du findest rechts oben bei den Intervall-Reglern jetzt den neuen einstellbaren Prozentwert für die Wort-Toleranz (Standard: 70%).
- Das hat den gigantischen Vorteil, dass die App nicht nur die Ränder sauber zusammenklebt, sondern auch OCR-Stotterer mitten im Satz (z. B. wenn die Kamera liest: "Das ist ist gut") sofort erkennt und killt!

 
KAPITEL 8: WÖRTERBUCH, AUTO-LEARN & DIE SCHATZTRUHE
 
Das Programm ist eine lernende KI, die sich selbst heilt.

8.1 Der Wörterbuch-Filter (`dictionary.txt`)
- Die App sucht beim Start jetzt nach einer Datei namens `dictionary.txt` im selben Ordner.
- Findet sie diese Datei und du setzt den neuen Haken bei "Wörterbuch-Filter", gleicht die App jedes Wort ab.
- Es ist ein absolut gnadenloser Bouncer. Wenn ein Wort mehr als 3 Buchstaben hat und NICHT exakt in deiner `wordlist-german.txt` steht, wird es sofort und ohne Rückfrage gelöscht (`Drop`). (Ausnahme: Alles mit Zahlen wird durchgelassen ).

8.2 Der Fuzzy-Agent (Überholspur / Rand-Reparatur)
- Wenn der Türsteher (Wörterbuch-Filter) ein Wort abweist (z. B. "tlassen"), wirft er es nicht mehr nur in die `drop.json`, sondern ruft heimlich den Agenten an.
- Er sucht jetzt ausschließlich nach großen Wörtern (Zielwort mindestens 10 Buchstaben), bei denen exakt am Anfang oder am Ende 1 bis 3 Buchstaben abgeschnitten wurden.
- Aus `genrennen` macht er sicher `Wagenrennen`.
- Der Agent stempelt das Wort sofort mit "Distanz 0" als perfekten Treffer ab, bricht die Suche auf der Stelle ab (spart extrem viel CPU-Zeit) und trägt es ins Lexikon ein. "okoladeneis" wird sofort zu "Schokoladeneis".

8.3 Auto-Learn (Sitzungs-Gedächtnis / 3-Strikes)
- Die App merkt sich jetzt im Hintergrund (mit einem unsichtbaren Zeitstempel), wann ein unbekanntes Wort auftaucht.
- Schlägt ein unbekanntes Wort (z.B. ein spezieller Nickname oder ein Slang-Wort wie "cringe") innerhalb von 10 Minuten 3 Mal auf, sagt der Türsteher: "Okay, das muss Absicht sein!".
- Er zählt einen "Strike" nur noch, wenn das Wort zwischenzeitlich vom Bildschirm verschwunden war. Bleibt ein Wort statisch stehen, wird es ignoriert und niemals gelernt.
- Er fügt das Wort sofort live in dein aktives Wörterbuch ein. Er merkt es sich nur noch im Arbeitsspeicher für diese eine Sitzung (bis du die App schließt oder "Löschen" drückst).

8.4 Die Meister-Datei (`custom_dictionary.json` / Phase 3)
Um das Lexikon dauerhaft zu trainieren, gibt es eine Master-Datei.
- Die neue `custom_dictionary.json` speichert alle unbekannten Wörter jetzt strukturiert als kleines Objekt ab. Der `Count` rattert hoch, aber das Wort bleibt auf `"Approved": false`.
- Deine Auswertung (Die Meister-Rolle): Du öffnest nach ein paar Tagen die JSON-Datei. Du siehst sofort, welche Wörter hohe Counts haben. Ist es ein gutes Wort, änderst du einfach das `false` in ein `true`.
- Beim Start lädt die App weiterhin brav deine `dictionary.txt`. Direkt danach liest sie die `custom_dictionary.json` ein. Sobald sie dort ein Wort sieht, bei dem `"Approved": true` steht, lädt sie es fest in den Arbeitsspeicher.

 
KAPITEL 9: AUDIO-ENGINE & ZAHLEN-KONVERTIERUNG
 
Damit die Ausgabe absolut flüssig ist, wird Text massiv vorverarbeitet.

9.1 Die Stimmen (Windows OneCore)
- Ich habe dafür die modernen Windows 10/11 OneCore-Schnittstellen (`Windows.Media.SpeechSynthesis` und `Windows.Media.Playback`) eingebaut.
- Neben dem Wörterbuch-Filter findest du jetzt zwei neue Dropdown-Menüs ("Stimme 1" und "Stimme 2").
- Die Sprachausgabe wechselt jetzt bei jedem gesprochenen Block automatisch zwischen Stimme 1 und Stimme 2.

9.2 Sprech-Tempo und Lautstärke
- Auf der rechten Seite (über dem Log) findest du jetzt das Feld `"Sprech-Tempo"`. Du kannst die Geschwindigkeit stufenlos von `0,5` (halbe Geschwindigkeit) bis `6,0` (6-fache Geschwindigkeit) einstellen.
- Direkt darunter sitzt jetzt ein moderner Slider (TrackBar) für die Lautstärke. Er reicht von 0 bis 100. Beide Regler greifen sofort live in den Audio-Strom ein!

9.3 Datums- und Zahlen-Verarbeitung
- Der Pre-Processor sucht jetzt nach Zahlenmustern wie `1.300` oder `1.500.000`, entfernt unsichtbar die Punkte und füttert sie als ganze Zahl an unsere Zahlen-Engine.
- Finde Datumsangaben in Formaten wie 'DD.MM.YYYY', 'YYYY.MM.DD' oder 'DD.MMM.YYYY' (z. B. 02.01.2026, 2026.01.02, 01.jan.2026) mithilfe von regulären Ausdrücken.
- Beachte den Kasus (Grammatik) anhand des optionalen Begleitwortes direkt vor dem Datum: Steht dort 'am', 'vom', 'zum', 'beim', 'den', 'dem', 'ab' oder 'seit', nutze die Endung '-ten'/'-sten' (z.B. 'am zweiundzwanzigsten'). Steht dort 'der', 'die' oder 'das', nutze die Endung '-te'/'-ste' (z.B. 'der zweiundzwanzigste').
- Das Jahr 2026 soll als 'zwanzigsechsundzwanzig' gesprochen werden, nicht als 'zweitausendsechsundzwanzig'.

 
KAPITEL 10: DEBUGGING & PROTOKOLLE (Röntgenblick)
 
Wenn du wissen willst, was die Maschine denkt, gibt es mehrere Diagnose-Fenster.

10.1 Das Synchrone Log ("Log kopieren")
- Die App führt jetzt im Hintergrund heimlich ein gigantisches Logbuch (bis zu 10.000 Einträge).
- Sobald Tesseract neuen Text liest, speichert sie das als `[RAW]`. Sobald ein Text alle deine Filter überlebt und in der Liste landet, speichert sie ihn mit exakt demselben Millisekunden-Zeitstempel als `[OUT]`.
- Ab sofort wird JEDER Eintrag im Log (`[RAW]`, `[BUF]`, `[OUT]`, `[DROP]`, `[SPK]`) exakt mit dem Eintrag verglichen, der eine Zeile vorher (vom selben Typ) ins Log geschrieben wurde. Du siehst jetzt in der Mitte des Logs z.B. `[RAW | 080%]`.

10.2 Overlap Debug
- Direkt daneben sitzt die Checkbox `Overlap Debug`. Wenn du sie aktivierst, schreibt die App jeden einzelnen Wort-Vergleich (z. B. Alt: 'hallo' vs Neu: 'hällo' -> 85%) heimlich mit.
- Wenn du das doppelte Vorlesen bemerkst, klickst du den Haken bei `Overlap Debug` einfach raus. In der Sekunde ploppt für dich das gesammelte Log in einem eigenen, gut lesbaren Fenster auf und leert danach den Speicher.

10.3 Wörterbuch Debug
- Direkt neben der Wörterbuch-Checkbox gibt es jetzt die neue Checkbox `"Wörterbuch Debug"`.
- Wenn du den Haken setzt, schreibt die App jede Entscheidung lautlos mit: `[WÖRTERBUCH] DROP: 'hlllo' (nicht gefunden)` oder `[WÖRTERBUCH] OK: 'hallo'`. Klickst du den Haken wieder raus, ploppt (genau wie beim Overlap-Debug) dein Log auf.
- Er schreibt dir dann stolz ins Log: `[FUZZY-AGENT] 'tlassen' -> automatisch korrigiert zu 'entlassen'`.

10.4 Lern-Historie & Info-Button
- Oben rechts neben dem Lautstärkeregler findest du jetzt zwei neue Buttons:
- Lern-Historie: Zeigt dir in einem Popup sofort alle Wörter, die der Agent in dieser Sitzung gelernt hat.
- Info / Hilfe: Ein ausführliches Handbuch mit Erklärungen zu Modus, Gedächtnis (ja, das Gedächtnis wirkt in beiden Modi!), Dice und Auto-Learn – natürlich in einer `RichTextBox`, damit du mit Strg+Mausrad zoomen kannst.

 

 
KAPITEL 11: MEISTER-FAQ & FEHLERBEHEBUNG
 
In diesem Kapitel findest du die Lösungen zu allen Hürden, Bugs und Besonderheiten, die während der Entwicklung der App aufgetreten sind. Es ist das gesammelte Wissen aus unzähligen Praxistests.

Frage 1: Die Blacklist arbeitet nicht gut. Im ListView tauchen die Wörter nicht auf, aber sie werden vorgelesen.
Antwort: Wir haben die Filter strikt in zwei Listen getrennt. Die Globale Blacklist (Ausschluss) ignoriert die Zeile komplett. Sie erscheint weder in der ListView, noch wird sie jemals vorgelesen. Die Sprach-Blacklist (Mute) lässt den Text in der ListView sichtbar, löscht die Wörter aber, bevor die Sprachausgabe startet.

Frage 2: Die ListView frisst die Zeilen. Der Text ist unten abgeschnitten.
Antwort: Wenn man die Schriftgröße in einer ListView (im Details-Modus) vergrößert, skaliert die Zeilenhöhe leider oft nicht automatisch mit. Der Trick: Wir weisen der ListView eine leere Bilderliste (ImageList) zu, die genau 1 Pixel breit und 30 Pixel hoch ist. Die ListView passt ihre Zeilenhöhe dann zwingend an dieses unsichtbare "Bild" an.

Frage 3: Die App gehorcht nicht. Es steht nirgends das Wort "Warenzeichen", aber trotzdem wird es vorgelesen.
Antwort: Das ist ein Trick der Windows-Sprachausgabe (TTS). Wenn die OCR ein Sonderzeichen wie "®" erfasst , übersetzt die Windows-Stimme das vollautomatisch als das Wort "Warenzeichen". Ein integrierter Sonderzeichen-Killer löscht vor der Sprachausgabe radikal alles, was kein echter Buchstabe, keine Zahl oder kein normales Satzzeichen ist.

Frage 4: Der Lesebereich springt plötzlich nach oben links (0,0) und macht sich selbstständig.
Antwort: Das ist ein WinForms-Bug bei DPI-Skalierungen (z.B. 125% Textgröße) oder mehreren Monitoren. Die App teilt Windows explizit mit, dass sie sich selbst skaliert (DPI-Awareness). Außerdem spannt der Auswahl-Modus ein unsichtbares Netz über den gesamten virtuellen Desktop. Der Rahmen selbst darf ab sofort niemals mehr den Fenster-Fokus annehmen (`ShowWithoutActivation`), damit Windows ihn beim Klicken von Buttons in Ruhe lässt.

Frage 5: Die Stimme holt mitten im Satz tief Luft (Schnappatmung) und liest Müll wie "UTUC UT CIUCTIE".
Antwort: Die OCR-Engine hat versteckte Zeilenumbrüche (\r\n) mitgelesen. Die Windows-Stimme wertet jeden unsichtbaren Zeilenumbruch als neuen Absatz und holt tief Luft. Die Funktion `CleanOcrText` killt alle Zeilenumbrüche und plättet alles zu einem flachen Stream. Ein intelligenter Anti-Lack-Filter filtert umherfliegende Buchstaben und Müll automatisch heraus.

Frage 6: Wie verarbeitet die App das riesige Wörterbuch (260.000 Einträge), ohne abzustürzen? 
Antwort: Die App nutzt ein HashSet, welches die O(1) Zeitkomplexität bietet. Anstatt eine Liste von oben nach unten abzusuchen, nutzt es eine clevere Hash-Tabelle, bei der die Überprüfung in Millisekunden passiert. Groß- und Kleinschreibung wird komplett ignoriert (`OrdinalIgnoreCase`).

Frage 7: Ich habe die App kompiliert, aber es ist noch der alte Code. Die neuen Buttons fehlen! 
Antwort: Wenn die alte .exe-Datei im Hintergrund abgestürzt ist oder noch unsichtbar läuft, kann der Compiler die Datei beim neuen Build nicht überschreiben. Er bricht heimlich ab und startet wieder die alte Version aus dem Cache. Lösung: Alle Prozesse der App im Task-Manager beenden oder den Namen der .exe-Datei in der Projektdatei (.csproj) anpassen.

Frage 8: Im TikTok-Modus stauen sich kurze Sätze und werden nicht sofort vorgelesen.
Antwort: Das Sprach-Vorzimmer hatte den Befehl, erst bei 8 Wörtern oder einem Satzzeichen flüssig vorzulesen. Bei YouTube-Untertiteln ist das super. Im TikTok-Modus kommen jedoch oft nur kurze Sätze ("Guten Morgen"). Die App hat nun eine Weiche eingebaut: Im TikTok-Modus gibt es kein Sammeln mehr, der Text wird sofort an die Lautsprecher geschickt.

Frage 9: Der FlyReader zittert beim Scrollen, die letzte Zeile ist manchmal versteckt.
Antwort: Da wir den Wagenrücklauf (\n) aus dem Text entfernt haben, "denkt" die RichTextBox, die Zeile sei noch nicht ganz fertig, und versteckt manchmal die unteren Pixel. Die Box bekommt nach jedem neuen Text jetzt den nativen Windows-Befehl (WM_VSCROLL -> SB_BOTTOM). Sie scrollt exakt ans absolute Ende des Inhalts, egal was passiert.

Frage 10: In der custom_dictionary.json stehen kryptische Zeichen (z.B. \u00F6) statt normaler Umlaute.
Antwort: Der .NET JsonSerializer wandelt Umlaute aus Sicherheitsgründen für Browser in Unicode-Sequenzen um. Wir haben den Befehl `UnsafeRelaxedJsonEscaping` eingebaut, der die App zwingt, ä, ö, ü und ß in den JSON-Dateien exakt als deutsche Buchstaben abzuspeichern.

Frage 11: Die App lernt aus Versehen falschen Text (Spam wie "uviusikl").
Antwort: Wenn die Kamera den Text wegen des Hintergrunds konstant falsch liest, zählte die alte 3-Strikes-Regel das hoch. Der Lern-Agent hat jetzt ein Echtzeit-Kurzzeitgedächtnis. Er zählt einen Strike nur noch, wenn das Wort zwischenzeitlich vom Bildschirm verschwunden war. Bleibt ein Fehler statisch stehen, wird er niemals gelernt.

Frage 12: Tausender-Zahlen (wie 1.300) zerschießen den Lesefluss.
Antwort: Der alte Filter zerlegte die Zahl am Trennpunkt in eine "1" und eine "300". Der Pre-Processor nutzt jetzt einen speziellen Tausender-Punkt Regex. Er entfernt unsichtbar die Punkte und füttert das Ergebnis als ganze Zahl an unsere Zahlen-Engine. "1.300" wird dann flüssig als "tausenddreihundert" vorgelesen.
 
MEISTER-FAQ
 
Frage: Wie genau funktioniert das Wörterbuch, wenn ein Wort darin nicht vorkommt, wird es ignoriert? 
Antwort: Es ist ein absolut gnadenloser Bouncer. Wenn ein Wort mehr als 3 Buchstaben hat und NICHT exakt in deiner `dictionary.txt` steht, wird es sofort und ohne Rückfrage gelöscht (`Drop`). Es korrigiert nichts! Eine Auto-Korrektur (wie am Handy) würde im Hintergrund jedes gelesene Wort mit 300.000 Wörterbuch-Einträgen auf Ähnlichkeit vergleichen. 

Frage: Ich habe ab und zu den Verdacht, dass die App Wörter vorliest, die garantiert nicht im Wörterbuch stehen. 
Antwort: Die OCR fotografiert den Bildschirm alle 888 Millisekunden. Wenn dort jetzt für drei Sekunden der Text `[Musik]` steht und die Kamera den Text wegen des Hintergrunds konstant falsch als `uviusikl` liest, hat die App den Fehler 4x hintereinander gelesen. Der alte Agent dachte sich: "Oh, das Wort kam 3x in den letzten 10 Minuten vor! Ich schalte es frei!" Die Lösung in V016: Der Agent hat jetzt ein Echtzeit-Kurzzeitgedächtnis. Er zählt einen "Strike" nur noch, wenn das Wort zwischenzeitlich vom Bildschirm verschwunden war. Bleibt ein Wort statisch stehen, wird es ignoriert und niemals gelernt. Das Spam-Problem ist gelöst!

