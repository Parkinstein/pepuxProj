/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){function t(e){var t=function(){};return t.prototype=e,t}var a=e._load;e._load=function(e,n){if(e=e||this._load_url,"object"==typeof e)for(var i=t(this._loaded),l=0;l<e.length;l++)this._loaded=new i,a.call(this,e[l],n);else a.apply(this,arguments)}}()});