   M S B A M L   `   `   `  ����  AuthorDetector'    !WpfApplication1.AddOrChangeCorpus   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � 
ResizeMode$  NoResize=�5   K    � Icon$ .\AuthorDetector3.ico��5   @    � Title$ Add Or Change Corpus��5   	   $	��350��6&   $	��318��63    � WindowStartupLocation$ CenterScreen=�6?   .��5      � .+�5   
   �� $+��%/Images/AuthorDetector3_grayscale.png��6   $��Center=�6?   �� 5   
   -   5        $��Add New Corpus��5      $
��Left=�6*   $��158,102,0,0q�6E   $	��Top=�6Z   $	��132��6r    � ToolTip 5      �� 5	      CAcorpus structure should be a directory containing author folders,�� 5
   W   =;each folder containg .txt documents written by that author.�� 5   Q   �� 6]   corpus�� 5      	--author1�� 5      ------doc1.txt�� 5   $   ------doc2.txt�� 5   $   	--author2�� 5      ------doc3.txt5      5      �� 5   
   -   5         ��Name$ btn_Add_Doc��5      $��Add Document��6&   $
��Left=�6=   $��158,202,0,0q�6X   $	��Top=�6m   $	��132��6�   6�   �� 5   
   -   5        $ 
btn_Cancel��5      $��Cancel��6%   $
��Left=�66   $��215,270,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   �� 5   
   $��Corpus��6   $
��Left=�6!   $	��Top=�6<   $	��310��6T   $��18(�6`   $��0,10,0,0q�6n   $
��Bold"�6�   $��Trebuchet MS)�6�   
���   ����	#FF98B2C9$��38��6�   �� 5   
   -   5         ��Name$ corpus_name��5      $
��Left=�6'   $��23��6B   $��
10,102,0,0q�6N    �� TextWrapping$
 Wrap=�6b   $�� ��6v   $	��Top=�6~   $	��115��6�   �� 5   
   $��Corpus Name��6   $��14(�6&   $
��Left=�64   $��	10,71,0,0q�6O   $	��Top=�6b   $	��115��6z   �� 5   
   $��Author Name��6   $��14(�6&   $
��Left=�64   $��
10,168,0,0q�6O   $	��Top=�6c   $	��115��6{   �� 5   
   $	��312��6   $��0,159,0,-151q�6$   $	��Top=�6:    J� RenderTransformOrigin$	 0,0&�6R   $��3��6n   $
��Left=�6x   z�5      V� 5      .~�5      �� �� 5       �� 5!      	 �� Angle$		 -90Q�6&   T� 5"      5#      5%      �� 5&   
   -   5        
 ��Name$
 author_combo��5&      $
��Left=�6)   $��
10,202,0,0q�6D   $	��Top=�6X   $	��115��6p   5(      5)      