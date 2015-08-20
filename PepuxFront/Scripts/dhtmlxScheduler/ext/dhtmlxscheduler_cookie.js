/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){function a(e,a,t){var l=e+"="+t+(a?"; "+a:"");document.cookie=l}function t(e){var a=e+"=";if(document.cookie.length>0){var t=document.cookie.indexOf(a);if(-1!=t){t+=a.length;var l=document.cookie.indexOf(";",t);return-1==l&&(l=document.cookie.length),document.cookie.substring(t,l)}}return""}var l=!0;e.attachEvent("onBeforeViewChange",function(n,i,o,r){if(l&&e._get_url_nav){var d=e._get_url_nav();(d.date||d.mode||d.event)&&(l=!1)}var _=(e._obj.id||"scheduler")+"_settings";

if(l){l=!1;var p=t(_);if(p){e._min_date||(e._min_date=r),p=unescape(p).split("@"),p[0]=this.templates.xml_date(p[0]);var c=this.isViewExists(p[1])?p[1]:o,u=isNaN(+p[0])?r:p[0];return window.setTimeout(function(){e.setCurrentView(u,c)},1),!1}}var s=escape(this.templates.xml_format(r||i)+"@"+(o||n));return a(_,"expires=Sun, 31 Jan 9999 22:00:00 GMT",s),!0});var n=e._load;e._load=function(){var a=arguments;if(!e._date&&e._load_mode){var t=this;window.setTimeout(function(){n.apply(t,a)},1)}else n.apply(this,a);

}}()});