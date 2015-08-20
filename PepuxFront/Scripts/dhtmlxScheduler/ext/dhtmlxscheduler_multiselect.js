/*
@license
dhtmlxScheduler.Net v.3.3.4 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.form_blocks.multiselect={render:function(e){for(var t="<div class='dhx_multi_select_"+e.name+"' style='overflow: auto; height: "+e.height+"px; position: relative;' >",a=0;a<e.options.length;a++)t+="<label><input type='checkbox' value='"+e.options[a].key+"'/>"+e.options[a].label+"</label>",convertStringToBoolean(e.vertical)&&(t+="<br/>");return t+="</div>"},set_value:function(t,a,n,i){function l(e){for(var a=t.getElementsByTagName("input"),n=0;n<a.length;n++)a[n].checked=!!e[a[n].value];

}for(var r=t.getElementsByTagName("input"),o=0;o<r.length;o++)r[o].checked=!1;var d={};if(n[i.map_to]){for(var _=(n[i.map_to]+"").split(i.delimiter||e.config.section_delimiter||","),o=0;o<_.length;o++)d[_[o]]=!0;l(d)}else{if(e._new_event||!i.script_url)return;var s=document.createElement("div");s.className="dhx_loading",s.style.cssText="position: absolute; top: 40%; left: 40%;",t.appendChild(s),dhtmlxAjax.get(i.script_url+"?dhx_crosslink_"+i.map_to+"="+n.id+"&uid="+e.uid(),function(e){for(var a=e.doXPath("//data/item"),n={},r=0;r<a.length;r++)n[a[r].getAttribute(i.map_to)]=!0;

l(n),t.removeChild(s)})}},get_value:function(t,a,n){for(var i=[],l=t.getElementsByTagName("input"),r=0;r<l.length;r++)l[r].checked&&i.push(l[r].value);return i.join(n.delimiter||e.config.section_delimiter||",")},focus:function(e){}}});