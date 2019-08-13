<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Web_Default2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <link href="../Scripts/jquery-ui-1.12.1.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../Scripts/jquery-ui-1.12.1.js"></script>

  <script>
      $(function () {
          $("input").checkboxradio({

          });
      });
  </script>
    <title></title>
</head>
<body>
<div class="widget" style="width:1024px">
  <h1>Checkbox and radio button widgets</h1>
 
  <h2>Radio Group</h2>
  <fieldset>
    <legend>Select a Location: </legend>
    <label for="radio-1">New York</label>
    <input type="radio" name="radio-1" id="radio-1">
    <label for="radio-2">Paris</label>
    <input type="radio" name="radio-1" id="radio-2">
    <label for="radio-3">London</label>
    <input type="radio" name="radio-1" id="radio-3">
  </fieldset>
 
  <h2>Checkbox</h2>
  <fieldset>
    <legend>Hotel Ratings: </legend>
    <label for="checkbox-1">2 Star</label>
    <input type="checkbox" name="checkbox-1" id="checkbox-1">
    <label for="checkbox-2">3 Star</label>
    <input type="checkbox" name="checkbox-2" id="checkbox-2">
    <label for="checkbox-3">4 Star</label>
    <input type="checkbox" name="checkbox-3" id="checkbox-3">
    <label for="checkbox-4">5 Star</label>
    <input type="checkbox" name="checkbox-4" id="checkbox-4">
  </fieldset>
 
  <h2>Checkbox nested in label</h2>
  <fieldset>
    <legend>Bed Type: </legend>
    <label for="checkbox-nested-1">2 Double
      <input type="checkbox" name="checkbox-nested-1" id="checkbox-nested-1">
    </label>
    <label for="checkbox-nested-2">2 Queen
      <input type="checkbox" name="checkbox-nested-2" id="checkbox-nested-2">
    </label>
    <label for="checkbox-nested-3">1 Queen
      <input type="checkbox" name="checkbox-nested-3" id="checkbox-nested-3">
    </label>
    <label for="checkbox-nested-4">1 King
      <input type="checkbox" name="checkbox-nested-4" id="checkbox-nested-4">
    </label>
  </fieldset>
  </fieldset>

    <br />

</div>
</body>
</html>
