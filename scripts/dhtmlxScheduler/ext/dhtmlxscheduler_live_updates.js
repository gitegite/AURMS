/*
@license
dhtmlxScheduler.Net v.3.3.6 

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){if("undefined"!=typeof dataProcessor){var t=dataProcessor.prototype.init;dataProcessor.prototype.init=function(){t.apply(this,arguments);var e=this;this.attachEvent("onAfterUpdate",function(t,i,a,n){var s;s=e.obj.exists(a)?e.obj.item(a):e.obj.exists(t)?e.obj.item(t):{},"undefined"!=typeof s.$selected&&delete s.$selected,"undefined"!=typeof s.$template&&delete s.$template,e.callEvent("onLocalUpdate",[{sid:t,tid:a,status:i,data:s}])})},dataProcessor.prototype.applyChanges=function(e){
var t=this,i=e.sid,a=e.tid,n=e.status,s=e.data;switch(t.obj.isSelected(i)&&(s.$selected=!0),n){case"updated":case"update":case"inserted":case"insert":t.obj.exists(i)?(t.obj.isLUEdit(s)===i&&t.obj.stopEditBefore(),t.ignore(function(){t.obj.update(i,s),i!==a&&t.obj.changeId(i,a)})):(s.id=a,t.ignore(function(){t.obj.add(s)}));break;case"deleted":case"delete":t.ignore(function(){var e=t.obj.exists(i);e&&(t.obj.setUserData(i,"!nativeeditor_status","true_deleted"),t.obj.stopEditBefore()),t.obj.remove(i,s),
e&&t.obj.isLUEdit(s)===i&&(t.obj.preventLUCollision(s),t.obj.callEvent("onLiveUpdateCollision",[i,a,n,s])===!1&&t.obj.stopEditAfter())})}}}"undefined"!=typeof e&&(e.item=function(t){var i=this.getEvent(t);if(!i)return{};var a={};for(var n in i)a[n]=i[n];return a.start_date=e.date.date_to_str(e.config.api_date)(i.start_date),a.end_date=e.date.date_to_str(e.config.api_date)(i.end_date),a},e.update=function(t,i){var a=this.getEvent(t);for(var n in i)"start_date"!=n&&"end_date"!=n&&(a[n]=i[n]);var s=e.date.str_to_date(e.config.api_date);

e.setEventStartDate(t,s(i.start_date)),e.setEventEndDate(t,s(i.end_date)),this.updateEvent(t),this.callEvent("onEventChanged",[t])},e.remove=function(t,i){if(this.exists(t)){var a=this.getEvent(t);if(this._get_rec_markers){a.rec_type&&this._roll_back_dates(a);var n=this._get_rec_markers(t);for(var s in n)n.hasOwnProperty(s)&&(t=n[s].id,this.getEvent(t)&&this.deleteEvent(t,!0))}this.deleteEvent(t,!0)}else i&&i.event_pid&&e.add(i)},e.exists=function(e){var t=this.getEvent(e);return t?!0:!1},e.add=function(e){
var t=this.addEvent(e.start_date,e.end_date,e.text,e.id,e);return this._is_modified_occurence&&this._is_modified_occurence(e)&&this.setCurrentView(),t},e.changeId=function(e,t){return this.changeEventId(e,t)},e.stopEditBefore=function(){},e.stopEditAfter=function(){this.endLightbox(!1,this._lightbox)},e.preventLUCollision=function(e){this._new_event=this._lightbox_id,e.id=this._lightbox_id,this._events[this._lightbox_id]=e},e.isLUEdit=function(e){return this._lightbox_id?this._lightbox_id:null},e.isSelected=function(e){
return!1})});