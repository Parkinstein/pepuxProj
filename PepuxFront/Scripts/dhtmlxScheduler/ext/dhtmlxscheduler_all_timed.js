/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){e.config.all_timed="short";var t=function(e){return!((e.end_date-e.start_date)/36e5>=24)};e._safe_copy=function(t){var a=null,n=null;return t.event_pid&&(a=e.getEvent(t.event_pid)),a&&a.isPrototypeOf(t)?(n=e._copy_event(t),delete n.event_length,delete n.event_pid,delete n.rec_pattern,delete n.rec_type):n=e._lame_clone(t),n};var a=e._pre_render_events_line;e._pre_render_events_line=function(n,_){function d(e){var t=i(e.start_date);return+e.end_date>+t}function i(t){
var a=e.date.add(t,1,"day");return a=e.date.date_part(a)}function r(t,a){var n=e.date.date_part(new Date(t));return n.setHours(a),n}if(!this.config.all_timed)return a.call(this,n,_);for(var l=0;l<n.length;l++){var s=n[l];if(!s._timed)if("short"!=this.config.all_timed||t(s)){var c=this._safe_copy(s);c.start_date=new Date(c.start_date),d(s)?(c.end_date=i(c.start_date),24!=this.config.last_hour&&(c.end_date=r(c.start_date,this.config.last_hour))):c.end_date=new Date(s.end_date);var o=!1;c.start_date<this._max_date&&c.end_date>this._min_date&&c.start_date<c.end_date&&(n[l]=c,
o=!0);var h=this._safe_copy(s);if(h.end_date=new Date(h.end_date),h.start_date=h.start_date<this._min_date?r(this._min_date,this.config.first_hour):r(i(s.start_date),this.config.first_hour),h.start_date<this._max_date&&h.start_date<h.end_date){if(!o){n[l--]=h;continue}n.splice(l+1,0,h)}}else n.splice(l--,1)}var u="move"==this._drag_mode?!1:_;return a.call(this,n,u)};var n=e.get_visible_events;e.get_visible_events=function(e){return this.config.all_timed&&this.config.multi_day?n.call(this,!1):n.call(this,e);

},e.attachEvent("onBeforeViewChange",function(t,a,n,_){return e._allow_dnd="day"==n||"week"==n,!0}),e._is_main_area_event=function(e){return!!(e._timed||this.config.all_timed===!0||"short"==this.config.all_timed&&t(e))};var _=e.updateEvent;e.updateEvent=function(t){var a,n=e.config.all_timed&&!(e.isOneDayEvent(e._events[t])||e.getState().drag_id);n&&(a=e.config.update_render,e.config.update_render=!0),_.apply(e,arguments),n&&(e.config.update_render=a)}}()});