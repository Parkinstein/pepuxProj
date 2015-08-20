/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.attachEvent("onTemplatesReady",function(){for(var a=document.body.getElementsByTagName("DIV"),t=0;t<a.length;t++){var n=a[t].className||"";if(n=n.split(":"),2==n.length&&"template"==n[0]){var l='return "'+(a[t].innerHTML||"").replace(/\"/g,'\\"').replace(/[\n\r]+/g,"")+'";';l=unescape(l).replace(/\{event\.([a-z]+)\}/g,function(e,a){return'"+ev.'+a+'+"'}),e.templates[n[1]]=Function("start","end","ev",l),a[t].style.display="none"}}})});