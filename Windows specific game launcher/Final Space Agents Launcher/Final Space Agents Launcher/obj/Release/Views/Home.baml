   M S B A M L   `   `   `  ???? V  RFinal Space Agents Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Final_Space_Agents_LauncherFinal_Space_Agents_Launcher  U/clr-namespace:Final_Space_Agents_Launcher.Views!Final_Space_Agents_Launcher.Views  ,    &Final_Space_Agents_Launcher.Views.Home   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     ;local)clr-namespace:Final_Space_Agents_Launcher     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Aviews/clr-namespace:Final_Space_Agents_Launcher.Views     -     >?Name$  Control??$
??Auto??$
??Auto?? ? 	Resources    ComboBoxTest2 ?? 
TargetType ?? Property ?? Value ?? 
TargetType d? 	IsChecked ?? Path ?? Mode	 ?? RelativeSource
 ? ColumnDefinitions ??Name ?? CornerRadius F?Name ?? Triggers P? Property P? Value ?? 
TargetName J? IsHitTestVisible ?? SelectionBoxItem ?? SelectionBoxItemTemplate ?? IsDropDownOpen ? AllowsTransparency J? SnapsToDevicePixels ? ActualWidth ?? MaxDropDownHeight K? IsItemsHost ?? Triggers??%#  &	        ?? " ??.???  ? ?? ?? " ??.R?? .+?d? $??2??|?.   ?? $ IsDropDownOpen?$ TwoWay=?	 ?? *TemplatedParent+???? .R?? 
 ?? $??5* ??? $??* ?.+??? $ Border??$??2??$ 5?????#141414
???   ?$??0q??? $??0??$ 5,0,0,5??$??1q????#141414
???   ?$??0,0,1,0q?._??? $??	  Version??$
??Left=?$??Center=????#7a00b2?? $??1??$ 5,0,0,5??$??1q????#141414
???   ?$??0,0,1,0q?._?F? $ Arrow??$??1?????#7a00b2$??Center=?$??Center=?"????        ? 	=  	= ? z     	 P?  ?@J?IsMouseOver . . ??? $ Border?? ? ?? ?#7a00b2P?  ?@d?	IsChecked$
 trueY?. ??? $ Border?? ? ?? ?#7a00b2? 
 ?? $??5* ??? $??* ?.+??? $??ContentSite?? .  $??0??$??Right=?$??Center=?#??z #??z #??z??$??3q?? $??Popup??$??Bottom=?#??z  . |?.  $
??Fade=?.??? $??DropDown?? . #??z #??z .+??? $ DropDownBorder?????#141414??  . .????  .  .????  ?? Padding ?? *OuterMarginSizeThickness+ ?? FallbackValue$ 10??._??  ? RowDefinitions ?? $
??234* ??? $
??301* ?.+??? -   $ 	Container??  LightAccentBrush#??[ $??2?? ?? *WindowCornerRadius+$ 10???? $??2?? ?? *WindowCornerRadius+???? *MainBackgroundBrush+$ #202020?? J? Effect 3  -System.Windows.Media.Effects.DropShadowEffect      ShadowDepth$  0Q?!   Opacity$	! 0.2Q?? $??2??" J? OpacityMask" !? # !? Visual# ?? $ ?? ElementName$$ 	Container??? ???#202020$??2??
 ?? $??30 ??? $	??40* ??? $
??137* ??? $??30 ? ?? $??30 ??? ?? $??30 ?.+??? -   $ ImageBorder??$??0??$??4??$??0??$??3??$ 10??$ 2q?._??? $??4??$??0??$??0??$??3??$
??Auto??$
??Auto??$!??/Images/SpaceAgentsHome.jpg??$??UniformToFill=??? -   $ Border2??$??1??$??2??$??1??$ 10??$ 2q?._??? -   $??1??$??1??$??0q????#7a00b2$??Italic$?$
??Bold"?$??20(?$
??Play??$??Bottom=?$??42??$??2?????? " ??.???  ? ?? ?#141414??  ? ?? ?? " ??.R??? #??z??._??? $??Center=?$??Center=? P?  ?@J?IsMouseOver . . ???  ? ??  BlueAccentBrush# [  ??%:   '??       ?? " ??.???  ?@??CornerRadius$ 10???? -   $??2??$??1?????#141414$??0q????#7a00b2$??Italic$?$	??150??$??22??$??Right=?$	??Top=?#??[  $??Fonts/#Orbitron)?$??
UltraBlack"?$??15(?(  "System.Windows.Controls.WebBrowser  -   $??3??$??1??$??Center=?$??Center=?$??15,220,15,255q?