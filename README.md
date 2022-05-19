# Apollo-Justice-Script-Editor
The script editor for Apollo Justice mobile port unpacked script. (Script unpacked with dlgTool)

This program is made for AA:AJ mobile port script. 

The font format is FontInfo + Atlas. This program contains all information about these files.
The font contains all glyphs from original mobile port + Russian glyphs.

The script, that this program works with, is extracted from the game with modded dlgTool (https://github.com/Akrasr/dlgTool-AJ-Android-Edition).

# How to replace the font?
You can replace font_info and font atlas files in resources of the project with the new ones.

# How to edit the font?
You can use my FontInfo editor: https://github.com/Akrasr/FontInfo-Editor.
The script characters' codes start with 0x90. And the characters codes in font_info are less by 0x90 (144) then in the script. For example, the G is saved in font_info as 61(0x3d) so in the script it is saved as 0xcd(201). After editing the font and placing it to the game, rewrite the aj3dsOrigCharConv dictionary is the way you need it. And update dlgTool's dictionary in the same way.
