/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){function t(t){var n=e._get_section_view();n&&t&&(a=e.getEvent(t)[e._get_section_property()])}var a,n;e.config.collision_limit=1,e.attachEvent("onBeforeDrag",function(e){return t(e),!0}),e.attachEvent("onBeforeLightbox",function(a){var i=e.getEvent(a);return n=[i.start_date,i.end_date],t(a),!0}),e.attachEvent("onEventChanged",function(t){if(!t||!e.getEvent(t))return!0;var a=e.getEvent(t);if(!e.checkCollision(a)){if(!n)return!1;a.start_date=n[0],a.end_date=n[1],
a._timed=this.isOneDayEvent(a)}return!0}),e.attachEvent("onBeforeEventChanged",function(t,a,n){return e.checkCollision(t)}),e.attachEvent("onEventAdded",function(t,a){var n=e.checkCollision(a);n||e.deleteEvent(t)}),e.attachEvent("onEventSave",function(t,a,n){if(a=e._lame_clone(a),a.id=t,!a.start_date||!a.end_date){var i=e.getEvent(t);a.start_date=new Date(i.start_date),a.end_date=new Date(i.end_date)}return a.rec_type&&e._roll_back_dates(a),e.checkCollision(a)}),e._check_sections_collision=function(t,a){
var n=e._get_section_property();return t[n]==a[n]&&t.id!=a.id?!0:!1},e.checkCollision=function(t){var n=[],i=e.config.collision_limit;if(t.rec_type)for(var _=e.getRecDates(t),d=0;d<_.length;d++)for(var r=e.getEvents(_[d].start_date,_[d].end_date),o=0;o<r.length;o++)(r[o].event_pid||r[o].id)!=t.id&&n.push(r[o]);else{n=e.getEvents(t.start_date,t.end_date);for(var l=0;l<n.length;l++)if(n[l].id==t.id){n.splice(l,1);break}}var c=e._get_section_view(),s=e._get_section_property(),v=!0;if(c){for(var h=0,l=0;l<n.length;l++)n[l].id!=t.id&&this._check_sections_collision(n[l],t)&&h++;

h>=i&&(v=!1)}else n.length>=i&&(v=!1);if(!v){var u=!e.callEvent("onEventCollision",[t,n]);return u||(t[s]=a||t[s]),u}return v}}()});