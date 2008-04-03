<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
<title>Loading Map Application</title> 
<style>
* { margin: 0; padding: 0; }
body { background-color: #95C1E8; }
#container { position: absolute; height: 100%; width: 100%; z-index: 1000; }
#container table { width: 100%; height: 100%; }
#container #content { text-align: center; vertical-align: middle; height: 100%; }
</style>    
</head>

<body>  
    <div id="container">
        <table>
            <tbody>
                <tr>
                    <td id="content">
                        <noscript>
                        Javascript not enabled.
                        </noscript>
                        <h3 id="loadingMessage" style="display: none; font-weight: bold;">
                        Please wait, application is loading...
                        </h3>
                        <script language="javascript" type="text/javascript">                          
                        var elm = document.getElementById('loadingMessage');
                        elm.style.display = '';
                        setTimeout(function() 
                        { 
                            var new_location = 'map.aspx' + window.location.search;
                            window.location = new_location;                        
                        }, 500);
                        </script>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>  
</body>
</html>
