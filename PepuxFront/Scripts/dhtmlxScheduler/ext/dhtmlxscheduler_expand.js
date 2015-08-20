/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.expand=function(){if(e.callEvent("onBeforeExpand",[])){var a=e._obj;do a._position=a.style.position||"",a.style.position="static";while((a=a.parentNode)&&a.style);a=e._obj,a.style.position="absolute",a._width=a.style.width,a._height=a.style.height,a.style.width=a.style.height="100%",a.style.top=a.style.left="0px";var t=document.body;t.scrollTop=0,t=t.parentNode,t&&(t.scrollTop=0),document.body._overflow=document.body.style.overflow||"",document.body.style.overflow="hidden",
e._maximize(),e.callEvent("onExpand",[])}},e.collapse=function(){if(e.callEvent("onBeforeCollapse",[])){var a=e._obj;do a.style.position=a._position;while((a=a.parentNode)&&a.style);a=e._obj,a.style.width=a._width,a.style.height=a._height,document.body.style.overflow=document.body._overflow,e._maximize(),e.callEvent("onCollapse",[])}},e.attachEvent("onTemplatesReady",function(){var a=document.createElement("DIV");a.className="dhx_expand_icon",e.toggleIcon=a,e._obj.appendChild(a),a.onclick=function(){
e.expanded?e.collapse():e.expand()}}),e._maximize=function(){this.expanded=!this.expanded,this.toggleIcon.style.backgroundPosition="0 "+(this.expanded?"0":"18")+"px";for(var a=["left","top"],t=0;t<a.length;t++){var n=(e.xy["margin_"+a[t]],e["_prev_margin_"+a[t]]);e.xy["margin_"+a[t]]?(e["_prev_margin_"+a[t]]=e.xy["margin_"+a[t]],e.xy["margin_"+a[t]]=0):n&&(e.xy["margin_"+a[t]]=e["_prev_margin_"+a[t]],delete e["_prev_margin_"+a[t]])}e.callEvent("onSchedulerResize",[])&&(e.update_view(),e.callEvent("onAfterSchedulerResize"));

}});