/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(t){t.config.active_link_view="day",t._active_link_click=function(e){var _=e.target||event.srcElement,i=_.getAttribute("jump_to"),n=t.date.str_to_date(t.config.api_date);return i?(t.setCurrentView(n(i),t.config.active_link_view),e&&e.preventDefault&&e.preventDefault(),!1):void 0},t.attachEvent("onTemplatesReady",function(){var e=function(e,_){_=_||e+"_scale_date",t.templates["_active_links_old_"+_]||(t.templates["_active_links_old_"+_]=t.templates[_]);var i=t.templates["_active_links_old_"+_],n=t.date.date_to_str(t.config.api_date);

t.templates[_]=function(t){return"<a jump_to='"+n(t)+"' href='#'>"+i(t)+"</a>"}};if(e("week"),e("","month_day"),this.matrix)for(var _ in this.matrix)e(_);this._detachDomEvent(this._obj,"click",t._active_link_click),dhtmlxEvent(this._obj,"click",t._active_link_click)})});