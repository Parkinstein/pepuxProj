/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e._get_serializable_data=function(){var e={};for(var t in this._events){var a=this._events[t];-1==a.id.toString().indexOf("#")&&(e[a.id]=a)}return e},e.data_attributes=function(){var t=[],a=e.templates.xml_format,n=this._get_serializable_data();for(var i in n){var r=n[i];for(var l in r)"_"!=l.substr(0,1)&&t.push([l,"start_date"==l||"end_date"==l?a:null]);break}return t},e.toXML=function(e){var t=[],a=this.data_attributes(),n=this._get_serializable_data();for(var i in n){
var r=n[i];t.push("<event>");for(var l=0;l<a.length;l++)t.push("<"+a[l][0]+"><![CDATA["+(a[l][1]?a[l][1](r[a[l][0]]):r[a[l][0]])+"]]></"+a[l][0]+">");t.push("</event>")}return(e||"")+"<data>"+t.join("\n")+"</data>"},e._serialize_json_value=function(e){return null===e||"boolean"==typeof e?e=""+e:(e||0===e||(e=""),e='"'+e.toString().replace(/\n/g,"").replace(/\\/g,"\\\\").replace(/\"/g,'\\"')+'"'),e},e.toJSON=function(){var e=[],t="",a=this.data_attributes(),n=this._get_serializable_data();for(var i in n){
for(var r=n[i],l=[],o=0;o<a.length;o++)t=a[o][1]?a[o][1](r[a[o][0]]):r[a[o][0]],l.push(' "'+a[o][0]+'": '+this._serialize_json_value(t));e.push("{"+l.join(",")+"}")}return"["+e.join(",\n")+"]"},e.toICal=function(t){var a="BEGIN:VCALENDAR\nVERSION:2.0\nPRODID:-//dhtmlXScheduler//NONSGML v2.2//EN\nDESCRIPTION:",n="END:VCALENDAR",i=e.date.date_to_str("%Y%m%dT%H%i%s"),r=e.date.date_to_str("%Y%m%d"),l=[],o=this._get_serializable_data();for(var d in o){var _=o[d];l.push("BEGIN:VEVENT"),l.push(_._timed&&(_.start_date.getHours()||_.start_date.getMinutes())?"DTSTART:"+i(_.start_date):"DTSTART:"+r(_.start_date)),
l.push(_._timed&&(_.end_date.getHours()||_.end_date.getMinutes())?"DTEND:"+i(_.end_date):"DTEND:"+r(_.end_date)),l.push("SUMMARY:"+_.text),l.push("END:VEVENT")}return a+(t||"")+"\n"+l.join("\n")+"\n"+n}});