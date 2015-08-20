/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e._temp_key_scope=function(){function a(e){delete e.rec_type,delete e.rec_pattern,delete e.event_pid,delete e.event_length}e.config.key_nav=!0;var t,n,l=null;e.attachEvent("onMouseMove",function(a,l){t=e.getActionData(l).date,n=e.getActionData(l).section}),e._make_pasted_event=function(l){var i=l.end_date-l.start_date,o=e._lame_copy({},l);if(a(o),o.start_date=new Date(t),o.end_date=new Date(o.start_date.valueOf()+i),n){var r=e._get_section_property();o[r]=e.config.multisection?l[r]:n;

}return o},e._do_paste=function(a,t,n){e.addEvent(t),e.callEvent("onEventPasted",[a,t,n])},e._is_key_nav_active=function(){return this._is_initialized()&&!this._is_lightbox_open()&&this.config.key_nav?!0:!1},dhtmlxEvent(document,_isOpera?"keypress":"keydown",function(a){if(!e._is_key_nav_active())return!0;if(a=a||event,37==a.keyCode||39==a.keyCode){a.cancelBubble=!0;var t=e.date.add(e._date,37==a.keyCode?-1:1,e._mode);return e.setCurrentView(t),!0}var n=e._select_id;if(a.ctrlKey&&67==a.keyCode)return n&&(e._buffer_id=n,
l=!0,e.callEvent("onEventCopied",[e.getEvent(n)])),!0;if(a.ctrlKey&&88==a.keyCode&&n){l=!1,e._buffer_id=n;var i=e.getEvent(n);e.updateEvent(i.id),e.callEvent("onEventCut",[i])}if(a.ctrlKey&&86==a.keyCode){var i=e.getEvent(e._buffer_id);if(i){var o=e._make_pasted_event(i);if(l)o.id=e.uid(),e._do_paste(l,o,i);else{var r=e.callEvent("onBeforeEventChanged",[o,a,!1,i]);r&&(e._do_paste(l,o,i),l=!0)}}return!0}})},e._temp_key_scope()});