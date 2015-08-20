/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){function t(e){var t={};for(var a in e)0!==a.indexOf("_")&&(t[a]=e[a]);return o.use_id||delete t.id,t}function a(){clearTimeout(r),r=setTimeout(function(){e.updateView()},1)}function n(e){e._loading=!0,e._not_render=!0,e.callEvent("onXLS",[])}function i(e){e._not_render=!1,e._render_wait&&e.render_view_data(),e._loading=!1,e.callEvent("onXLE",[])}function l(e){return o.use_id?e.id:e.cid}var r,o={use_id:!1};e.backbone=function(r,d){function _(){s.length&&(e.parse(s,"json"),
s=[])}d&&(o=d),r.bind("change",function(t,n){var i=l(t),r=e._events[i]=t.toJSON();r.id=i,e._init_event(r),a()}),r.bind("remove",function(t,a){var n=l(t);e._events[n]&&e.deleteEvent(n)});var s=[];r.bind("add",function(t,a){var n=l(t);if(!e._events[n]){var i=t.toJSON();i.id=n,e._init_event(i),s.push(i),1==s.length&&setTimeout(_,1)}}),r.bind("request",function(t){t instanceof Backbone.Collection&&n(e)}),r.bind("sync",function(t){t instanceof Backbone.Collection&&i(e)}),r.bind("error",function(t){t instanceof Backbone.Collection&&i(e);

}),e.attachEvent("onEventCreated",function(t){var a=new r.model(e.getEvent(t));return e._events[t]=a.toJSON(),e._events[t].id=t,!0}),e.attachEvent("onEventAdded",function(a){if(!r.get(a)){var n=t(e.getEvent(a)),i=new r.model(n),o=l(i);o!=a&&this.changeEventId(a,o),r.add(i),r.trigger("scheduler:add",i)}return!0}),e.attachEvent("onEventChanged",function(a){var n=r.get(a),i=t(e.getEvent(a));return n.set(i),r.trigger("scheduler:change",n),!0}),e.attachEvent("onEventDeleted",function(e){var t=r.get(e);

return t&&(r.trigger("scheduler:remove",t),r.remove(e)),!0})}}()});