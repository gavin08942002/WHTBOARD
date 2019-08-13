<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DialogTest.aspx.vb" Inherits="JQuery_DialogTest" %>

<!DOCTYPE html>

<html>
<head>
  <meta charset="utf-8">
  
  <title>jQuery UI Dialog - Animation</title>
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="../Web/Jquery-ui-1.12.1.custom/jquery-ui.js"></script>
    <link href="../Web/Jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet" />

  <script>
      $(function () {
          $("#dialog").dialog({
              modal: true,
              autoOpen: false,
              show: {
                  effect: "blind",
                  duration: 500,
              },
              hide: {
                  effect: "blind",
                  duration: 500
              }
          });

          $("#opener").on("click", function () {
              $("#dialog").dialog("open");
          });
      });
  </script>
</head>
<body>
 
<div id="dialog" title="Basic dialog">
  <p>This is an animated dialog which is useful for displaying information. The dialog window can be moved, resized and closed with the 'x' icon.</p>
</div>
 
<button id="opener">Open Dialog</button>
 
 
</body>
</html>
