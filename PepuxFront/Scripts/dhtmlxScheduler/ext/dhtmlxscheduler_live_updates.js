/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){if("undefined"!=typeof dataProcessor){var a=dataProcessor.prototype.init;dataProcessor.prototype.init=function(){a.apply(this,arguments);var e=this;this.attachEvent("onAfterUpdate",function(a,t,n,i){var l;l=e.obj.exists(n)?e.obj.item(n):e.obj.exists(a)?e.obj.item(a):{},"undefined"!=typeof l.$selected&&delete l.$selected,"undefined"!=typeof l.$template&&delete l.$template,e.callEvent("onLocalUpdate",[{sid:a,tid:n,status:t,data:l}])})},dataProcessor.prototype.applyChanges=function(e){
var a=this,t=e.sid,n=e.tid,i=e.status,l=e.data;switch(a.obj.isSelected(t)&&(l.$selected=!0),i){case"updated":case"update":case"inserted":case"insert":a.obj.exists(t)?(a.obj.isLUEdit(l)===t&&a.obj.stopEditBefore(),a.ignore(function(){a.obj.update(t,l),t!==n&&a.obj.changeId(t,n)})):(l.id=n,a.ignore(function(){a.obj.add(l)}));break;case"deleted":case"delete":a.ignore(function(){var e=a.obj.exists(t);e&&(a.obj.setUserData(t,"!nativeeditor_status","true_deleted"),a.obj.stopEditBefore()),a.obj.remove(t,l),
e&&a.obj.isLUEdit(l)===t&&(a.obj.preventLUCollision(l),a.obj.callEvent("onLiveUpdateCollision",[t,n,i,l])===!1&&a.obj.stopEditAfter())})}}}"undefined"!=typeof e&&(e.item=function(a){var t=this.getEvent(a);if(!t)return{};var n={};for(var i in t)n[i]=t[i];return n.start_date=e.date.date_to_str(e.config.api_date)(t.start_date),n.end_date=e.date.date_to_str(e.config.api_date)(t.end_date),n},e.update=function(a,t){var n=this.getEvent(a);for(var i in t)"start_date"!=i&&"end_date"!=i&&(n[i]=t[i]);var l=e.date.str_to_date(e.config.api_date);

e.setEventStartDate(a,l(t.start_date)),e.setEventEndDate(a,l(t.end_date)),this.updateEvent(a),this.callEvent("onEventChanged",[a])},e.remove=function(a,t){if(this.exists(a)){var n=this.getEvent(a);n.rec_type&&this._roll_back_dates(n);var i=this._get_rec_markers(a);for(var l in i)i.hasOwnProperty(l)&&(a=i[l].id,this.getEvent(a)&&this.deleteEvent(a,!0));this.deleteEvent(a,!0)}else t&&t.event_pid&&e.add(t)},e.exists=function(e){var a=this.getEvent(e);return a?!0:!1},e.add=function(e){var a=this.addEvent(e.start_date,e.end_date,e.text,e.id,e);

return this._is_modified_occurence(e)&&this.setCurrentView(),a},e.changeId=function(e,a){return this.changeEventId(e,a)},e.stopEditBefore=function(){},e.stopEditAfter=function(){this.endLightbox(!1,this._lightbox)},e.preventLUCollision=function(e){this._new_event=this._lightbox_id,e.id=this._lightbox_id,this._events[this._lightbox_id]=e},e.isLUEdit=function(e){return this._lightbox_id?this._lightbox_id:null},e.isSelected=function(e){return!1})});