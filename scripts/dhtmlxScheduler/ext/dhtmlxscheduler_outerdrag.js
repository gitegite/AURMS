/*
@license
dhtmlxScheduler.Net v.3.3.6 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){e.attachEvent("onTemplatesReady",function(){var t,a=new dhtmlDragAndDropObject,n=a.stopDrag;a.stopDrag=function(e){return t=e||event,n.apply(this,arguments)},a.addDragLanding(e._els.dhx_cal_data[0],{_drag:function(a,n,i,r){if(!e.checkEvent("onBeforeExternalDragIn")||e.callEvent("onBeforeExternalDragIn",[a,n,i,r,t])){var d=e.attachEvent("onEventCreated",function(n){e.callEvent("onExternalDragIn",[n,a,t])||(this._drag_mode=this._drag_id=null,this.deleteEvent(n))}),o=e.getActionData(t),l={
start_date:new Date(o.date)};if(e.matrix&&e.matrix[e._mode]){var s=e.matrix[e._mode];l[s.y_property]=o.section;var _=e._locate_cell_timeline(t);l.start_date=s._trace_x[_.x],l.end_date=e.date.add(l.start_date,s.x_step,s.x_unit)}e._props&&e._props[e._mode]&&(l[e._props[e._mode].map_to]=o.section),e.addEventNow(l),e.detachEvent(d)}},_dragIn:function(e,t){return e},_dragOut:function(e){return this}})})});