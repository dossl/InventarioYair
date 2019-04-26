<%@Page Language="C#" AutoEventWireup="true" CodeBehind="Mensajeria.aspx.cs" Inherits="InventarioOtro.Reportes.Mensajeria" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<!doctype html>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reportes - ICGES</title>
  
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="scriptManagerReport" runat="server" ScriptMode="Release"></asp:ScriptManager>

            <rsweb:ReportViewer runat="server" ShowPrintButton="true"
                Width="99.9%" Height="100%"  ZoomMode="Percent" KeepSessionAlive="true" SizeToReportContent="True" AsyncRendering="false"
                ID="rptViewer">
            </rsweb:ReportViewer>
          
        </div>
    </form>
</body>
</html>