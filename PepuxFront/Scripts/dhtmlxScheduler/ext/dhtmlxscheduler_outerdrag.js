/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.attachEvent("onTemplatesReady",function(){var t,a=new dhtmlDragAndDropObject,n=a.stopDrag;a.stopDrag=function(e){return t=e||event,n.apply(this,arguments)},a.addDragLanding(e._els.dhx_cal_data[0],{_drag:function(a,n,i,l){if(!e.checkEvent("onBeforeExternalDragIn")||e.callEvent("onBeforeExternalDragIn",[a,n,i,l,t])){var r=e.attachEvent("onEventCreated",function(n){e.callEvent("onExternalDragIn",[n,a,t])||(this._drag_mode=this._drag_id=null,this.deleteEvent(n))}),o=e.getActionData(t),d={
start_date:new Date(o.date)};if(e.matrix&&e.matrix[e._mode]){var _=e.matrix[e._mode];d[_.y_property]=o.section;var s=e._locate_cell_timeline(t);d.start_date=_._trace_x[s.x],d.end_date=e.date.add(d.start_date,_.x_step,_.x_unit)}e._props&&e._props[e._mode]&&(d[e._props[e._mode].map_to]=o.section),e.addEventNow(d),e.detachEvent(r)}},_dragIn:function(e,t){return e},_dragOut:function(e){return this}})})});