function MapToolTip(){
this.offsetxpoint=0;
this.offsetypoint=20;
this.ie=document.all;
this.ns6=document.getElementById&&!document.all;
this.enabletip=false;
this.isTracking=false;
this.iebody=function(){
return (document.compatMode&&document.compatMode!="BackCompat")?document.documentElement:document.body;
};
this.S=function(_1,_2){
if(this.ns6||this.ie){
this.tipobj=document.all?document.all[_2]:document.getElementById?document.getElementById(_2):"";
this.tipobj.innerHTML=_1;
this.enabletip=true;
}
return false;
};
this.M=function(e,_4){
if(!this.enabletip){
return;
}
if(typeof (_4)!=="undefined"){
this.isTracking=true;
var ev=e||event;
ev.stopPropagation?ev.stopPropagation():(ev.cancelBubble=true);
}else{
this.isTracking=false;
}
var _6=(this.ns6)?e.pageX:event.clientX+this.iebody().scrollLeft;
var _7=(this.ns6)?e.pageY:event.clientY+this.iebody().scrollTop;
var _8=this.ie&&!window.opera?this.iebody().clientWidth-event.clientX-this.offsetxpoint:window.innerWidth-e.clientX-this.offsetxpoint-20;
var _9=this.ie&&!window.opera?this.iebody().clientHeight-event.clientY-this.offsetypoint:window.innerHeight-e.clientY-this.offsetypoint-20;
var _a=(this.offsetxpoint<0)?this.offsetxpoint*(-1):-1000;
if(_8<this.tipobj.offsetWidth){
this.tipobj.style.left=this.ie?this.iebody().scrollLeft+event.clientX-this.tipobj.offsetWidth+"px":window.pageXOffset+e.clientX-this.tipobj.offsetWidth+"px";
}else{
if(_6<_a){
this.tipobj.style.left="5px";
}else{
this.tipobj.style.left=_6+this.offsetxpoint+"px";
}
}
if(_9<this.tipobj.offsetHeight){
this.tipobj.style.top=this.ie?this.iebody().scrollTop+event.clientY-this.tipobj.offsetHeight-10+"px":window.pageYOffset+e.clientY-this.tipobj.offsetHeight-10+"px";
}else{
this.tipobj.style.top=_7+this.offsetypoint+"px";
}
this.tipobj.style.visibility="visible";
};
this.H=function(){
if((this.ns6||this.ie)&&this.enabletip){
this.enabletip=false;
this.tipobj.style.visibility="hidden";
this.tipobj.style.left="-1000px";
}
this.enabletip=false;
this.isTracking=false;
};
this.hideToolpipsFromTracking=function(){
if(this.enabletip&&this.isTracking){
this.H();
}
};
}
var __TT=new MapToolTip();

