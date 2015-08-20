/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){e.config.container_autoresize=!0,e.config.month_day_min_height=90;var a=e._pre_render_events,t=!0;e._pre_render_events=function(l,n){if(!e.config.container_autoresize||!t)return a.apply(this,arguments);var i=this.xy.bar_height,o=this._colsS.heights,r=this._colsS.heights=[0,0,0,0,0,0,0],d=this._els.dhx_cal_data[0];if(l=this._table_view?this._pre_render_events_table(l,n):this._pre_render_events_line(l,n),this._table_view)if(n)this._colsS.heights=o;else{var _=d.firstChild;

if(_.rows){for(var p=0;p<_.rows.length;p++){if(r[p]++,r[p]*i>this._colsS.height-this.xy.month_head_height){var c=_.rows[p].cells,u=this._colsS.height-this.xy.month_head_height;1*this.config.max_month_events!==this.config.max_month_events||r[p]<=this.config.max_month_events?u=r[p]*i:(this.config.max_month_events+1)*i>this._colsS.height-this.xy.month_head_height&&(u=(this.config.max_month_events+1)*i);for(var s=0;s<c.length;s++)c[s].childNodes[1].style.height=u+"px";r[p]=(r[p-1]||0)+c[0].offsetHeight;

}r[p]=(r[p-1]||0)+_.rows[p].cells[0].offsetHeight}r.unshift(0),_.parentNode.offsetHeight<_.parentNode.scrollHeight&&!_._h_fix}else if(l.length||"visible"!=this._els.dhx_multi_day[0].style.visibility||(r[0]=-1),l.length||-1==r[0]){var v=(_.parentNode.childNodes,(r[0]+1)*i+1+"px");d.style.top=this._els.dhx_cal_navline[0].offsetHeight+this._els.dhx_cal_header[0].offsetHeight+parseInt(v,10)+"px",d.style.height=this._obj.offsetHeight-parseInt(d.style.top,10)-(this.xy.margin_top||0)+"px";var b=this._els.dhx_multi_day[0];

b.style.height=v,b.style.visibility=-1==r[0]?"hidden":"visible",b=this._els.dhx_multi_day[1],b.style.height=v,b.style.visibility=-1==r[0]?"hidden":"visible",b.className=r[0]?"dhx_multi_day_icon":"dhx_multi_day_icon_small",this._dy_shift=(r[0]+1)*i,r[0]=0}}return l};var l=["dhx_cal_navline","dhx_cal_header","dhx_multi_day","dhx_cal_data"],n=function(a){for(var t=0,n=0;n<l.length;n++){var i=l[n],o=e._els[i]?e._els[i][0]:null,r=0;switch(i){case"dhx_cal_navline":case"dhx_cal_header":r=parseInt(o.style.height,10);

break;case"dhx_multi_day":r=o?o.offsetHeight:0,1==r&&(r=0);break;case"dhx_cal_data":var d=e.getState().mode;if(r=o.childNodes[1]&&"month"!=d?o.childNodes[1].offsetHeight:Math.max(o.offsetHeight-1,o.scrollHeight),"month"==d){if(e.config.month_day_min_height&&!a){var _=o.getElementsByTagName("tr").length;r=_*e.config.month_day_min_height}a&&(o.style.height=r+"px")}if(e.matrix&&e.matrix[d])if(a)r+=2,o.style.height=r+"px";else{r=2;for(var p=e.matrix[d],c=p.y_unit,u=0;u<c.length;u++)r+=c[u].children?p.folder_dy||p.dy:p.dy;

}("day"==d||"week"==d)&&(r+=2)}t+=r}e._obj.style.height=t+"px",a||e.updateView()},i=function(){if(!e.config.container_autoresize||!t)return!0;var a=e.getState().mode;n(),(e.matrix&&e.matrix[a]||"month"==a)&&window.setTimeout(function(){n(!0)},1)};e.attachEvent("onViewChange",i),e.attachEvent("onXLE",i),e.attachEvent("onEventChanged",i),e.attachEvent("onEventCreated",i),e.attachEvent("onEventAdded",i),e.attachEvent("onEventDeleted",i),e.attachEvent("onAfterSchedulerResize",i),e.attachEvent("onClearAll",i),
e.attachEvent("onBeforeExpand",function(){return t=!1,!0}),e.attachEvent("onBeforeCollapse",function(){return t=!0,!0})}()});