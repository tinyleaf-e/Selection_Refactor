function _LogWrite(serverUrl,projectTicket,formatId,logData){
	logData.formatId=formatId;
	logData.projectTicket=projectTicket;
	
	var xmlHttp;
    try {
        // Firefox, Opera 8.0+, Safari
        xmlHttp = new XMLHttpRequest();
        } 
    catch (e) {
        // Internet Explorer
        try {
                xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
            try {
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) {
                alert("您的浏览器不支持AJAX！");
                return false;
            }
        }
    }
	
    xmlHttp.open( "post" , url , true );
    xmlHttp.setRequestHeader( "Content-Type" , "application/x-www-form-urlencoded" );
    xmlHttp.onreadystatechange = function () {
        if( xmlHttp.readyState == 4 ) {
            if( xmlHttp.status == 200 ) {
                console.log("Log Success");
            }
            else {
                console.log( "Log Fail 错误码："+xmlHttp.status );
            }
        }
    }
    xmlHttp.send( logData );
}