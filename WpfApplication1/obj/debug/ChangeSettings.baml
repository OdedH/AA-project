   M S B A M L   `   `   `  ����  AuthorDetector$    WpfApplication1.ChangeSettings   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � 
ResizeMode$  NoResize=�5   K    � Icon$ .\AuthorDetector3.ico��5   @    � Title$ Change Settings��5   	   $	��350��6!   $	��318��6.    � WindowStartupLocation$ CenterScreen=�6:   .��5      � .+�5   
   �� $+��%/Images/AuthorDetector3_grayscale.png��6   $��Center=�6?   �� 5   
   $��Corpus��6   $
��Left=�6!   $��	10,65,0,0q�6<   $	��Top=�6O   $��30��6g   $��85��6s   $��16(�6~   �� 5   
   -   5        $��ComboBox_Corpus��5      $
��Left=�6*   $��
159,65,0,0q�6E   $	��Top=�6Y   $	��120��6q   6�   �� 5	   
   $��Runtime��6    � ToolTip$- 'Choose maximal runtime for the algoritm��6"   $
��Left=�6T   $��
10,106,0,0q�6o   $	��Top=�6�   $��32��6�   $��85��6�   $��16(�6�   �� 5
   
   -   5        $��ComboBox_RunTime��5
      $
��Left=�6+   $��159,106,0,0q�6F   $	��Top=�6[   $	��120��6s   $��32��6   .@�5      �� -   5         ��Name$ Fast_CB��5       �� 
IsSelected . .��6>   Fast6D   �� 5      -   5        $ Slow_CB��5      .��6,   Slow62   5      �� 5   
   $��Features��6   $- 'Choose maximal runtime for the algoritm��6#   $
��Left=�6U   $��
10,153,0,0q�6p   $	��Top=�6�   $��30��6�   $��85��6�   $��16(�6�   �� 5   
   -   5        $��slider_Features��5      $
��Left=�6)    K� ZIndex$ 10��6D   $��118,160,0,0q�6V   $	��Top=�6k   $��87��6�   $��17��6�   �� 5   
    �� TextWrapping$
 Wrap=�6   $��232,153,14,137q�6D   	 �� 
IsReadOnly	 . ��6&   �� *FeatureValue+6\   �� 5   
   $��min variance
per feature��6   $- 'Choose maximal runtime for the algoritm��67   $
��Left=�6i   $��
10,193,0,0q�6�   $	��Top=�6�   $��55��6�   $	��143��6�   $��16(�6�   �� 5   
   -   5        $��slider_Variance��5      $
��Left=�6)   $ 10��6D   $��118,219,0,0q�6V   $	��Top=�6k   $��87��6�   $��17��6�   �� 5   
   $
 Wrap=�6   $��232,211,14,79q�6E   	 . ��6&   �� *VarianceValue+6\   �� 5   
   $
��Left=�6   $	��Top=�6+   $	��310��6C   $��18(�6O   $��0,10,0,0q�6]   $
��Bold"�6o   $��Trebuchet MS)�6�   
���   ����	#FF98B2C9$��38��6�   .��5      � .+�5      �� 
Settings6&   �� 5      
 �� TextAlignment$
 Right=�6   $��Italic$�62   $��12(�6E   $��
95,0,-38,0q�6S   #!Note: These settings changes will�� 6�   $"affect the result of the algorithm6�   5      5      �� 5   
   -   5         ��Name$ 	btn_Learn��5      $��Learn��6$   $
��Left=�64   $��
11,270,0,0q�6O   $	��Top=�6c   $��75��6{   6�   �� 5   
   -   5        $ btn_save��5      $
��Save��6#   $
��Left=�62   $��113,270,0,0q�6M   $	��Top=�6b   $��75��6z   6�   �� 5   
   -	   5        $ 
btn_Cancel��5      $��Cancel��6%   $
��Left=�66   $��215,270,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   5      5      