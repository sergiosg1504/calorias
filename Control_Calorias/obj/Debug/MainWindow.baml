   M S B A M L   `   `   `  ����K  GControl_Calorias, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Control_CaloriasControl_Calorias  5   	   !    Control_Calorias.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Control_Calorias     5   	   -   5        $��miMainWindow��5   	     � Title$  Control de Calorias��5	   	   $��Verdana)�6%   $	��900��6:   $
��1600��6G    � WindowStartupLocation$ CenterScreen=�6u   .��5
      �  � RowDefinitions 5   
   �� 5      $��0.025* �6   �� 5      $��0.05* �6   �� 5      .+�5   
   �� -   5        $��Promedio_Diario��5      $��Center=�6'   $��Center=�6D   $��1��6_   $y�Hidden=�6l   �� 5   
   -   5        $��	NightMode��5      $
��🌙��6"   $��Right=�6G   $��Center=�6c   $��0,0,40,0q�6~   $��30��6�   $��30��6�   $��1��6�   �� 5   
   -   5        $��Volver��5      $��Ver todo��6   $��Center=�6G   $��Center=�6d   $��30��6   $	��200��6�   $��1��6�   $y�Hidden=�6�   ]� 5   
   $��0��6   .+�5      o� $	��Top=�6   
�������.@�5      m� $
��Menu��6   $��DemiBold"�6)   .@�5      m� -   5        $��Archivo��5      60   m� 5      -   5        $��Control_VerTodos��5   1   $��Ver todo��6   6I   m� 5      -   5        $��Control_Cuadricula��5   ;   $��Activar cuadricula��6   6U   m� 5      -   5        $��Exportar��5      61   m� 5      -	   5        $��Importar��5      61   5      5      5      � 5   
   -
   5        $��miGridCanvas��5      $��2��6#   $��
25,0,25,35q�60   .+�5      �� $��2��6    J� 	AllowDrop . $
��Auto=�69   $
��Auto=�6^   .��5       �� -   5        $��miCanvas��5       $
��Auto=�6)   $
��Auto=�6[   ��. 
�������$y�Visible=�6�   6�   5!      5"      �� 5#   
   -   5        $��miCanvasTodos��5#      $��
25,0,25,35q�6&   $��2��6:   
�������$y�Hidden=�6Z   �� 5$   
   -   5        $��miCanvasDia��5$      $��
25,0,25,35q�6$   $��2��68   
�������$y�Hidden=�6X   5%      5&      