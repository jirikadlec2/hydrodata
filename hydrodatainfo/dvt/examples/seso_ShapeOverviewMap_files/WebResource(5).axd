function __doPanZoom(_1,_2,_3,_4){
var _5=null;
try{
_5=eval(_1);
}
catch(ex){
}
if(_5==null){
return;
}
var _6=__getMapTools(_2);
if(_6==null){
return;
}
var _7=_6.GetZoom();
switch(_3){
case "ZoomFull":
_5.updateThumb(0);
break;
case "ZoomIn":
_5.updateThumb(_7+1);
break;
case "ZoomOut":
_5.updateThumb(_7-1);
break;
case "SetZoom":
_5.updateThumb(_4);
break;
}
_6[_3](_4);
}
function __UpdateAnimationLayer(_8,_9){
__amUpdateEventValidation();
if(!__amIsAspAjaxCall){
try{
var _a=_8.split("``");
if(_a[0]=="LAYER"){
var _b=__$(_9+"_animdiv");
__clearDiv(_b);
_b.innerHTML=_a[2];
__$(_9+"_animarg").value=_a[1];
}else{
var _c=__$(_9);
__clearDiv(_c);
_c.innerHTML=_a[1];
var _d=__getMapTools(_9);
if(_d){
_d.UpdateBackLayer();
}
}
}
catch(e){
}
}
var _d=__getMapTools(_9);
if(!_d){
return;
}
if(_d.GetEnableAnimation()){
_d.RunAnimationTimer();
}
}
function __UpdateAnimationLayerError(_e,_f){
__amUpdateEventValidation();
var map=__getMapTools(_f);
if(!map){
return;
}
if(map.GetEnableAnimation()){
map.RunAnimationTimer();
}
}
function __AspMapTools(){
this.ZOOMFULL=100;
this.PIXELPAN=101;
this.ZOOMIN=102;
this.ZOOMOUT=103;
this.SETZOOM=104;
this.CENTERAT=105;
this.SETSCALE=106;
this.CENTERANDZOOM=107;
this.CENTERANDSCALE=108;
this.RESIZETO=109;
this.mouseMoveHandler=null;
this.panToolHandler=null;
this.pointToolHandler=null;
this.infoToolHandler=null;
this.markerClickHandler=null;
this.apiVar=null;
this.Add_MouseMove=function(_11){
this.mouseMoveHandler=_11;
};
this.Remove_MouseMove=function(_12){
this.mouseMoveHandler=null;
};
this.Add_PanToolClick=function(_13){
this.panToolHandler=_13;
};
this.Remove_PanToolClick=function(_14){
this.panToolHandler=null;
};
this.Add_PointTool=function(_15){
this.pointToolHandler=_15;
};
this.Remove_PointTool=function(_16){
this.pointToolHandler=null;
};
this.Add_InfoTool=function(_17){
this.infoToolHandler=_17;
};
this.Remove_InfoTool=function(_18){
this.infoToolHandler=null;
};
this.Add_MarkerClick=function(_19){
this.markerClickHandler=_19;
};
this.Remove_MarkerClick=function(_1a){
this.markerClickHandler=null;
};
this.PanLeft=function(_1b){
this.Pan(-this.MapWidth()*_1b/100,0);
};
this.PanTop=function(_1c){
this.Pan(0,-this.MapHeight()*_1c/100);
};
this.PanRight=function(_1d){
this.Pan(this.MapWidth()*_1d/100,0);
};
this.PanBottom=function(_1e){
this.Pan(0,this.MapHeight()*_1e/100);
};
this.ZoomFull=function(){
this.ApiCall(this.ZOOMFULL);
};
this.ZoomIn=function(){
this.ApiCall(this.ZOOMIN);
};
this.ZoomOut=function(){
this.ApiCall(this.ZOOMOUT);
};
this.Pan=function(dx,dy){
this.ApiCallXY(this.PIXELPAN,"",dx,dy);
};
this.Refresh=function(){
this.Pan(0,0);
};
this.CenterAt=function(x,y){
this.ApiCallXY(this.CENTERAT,"",x,y);
};
this.CenterAndZoom=function(x,y,_25){
this.ApiCallXY(this.CENTERANDZOOM,_25,x,y);
};
this.CenterAndScale=function(x,y,_28){
this.ApiCallXY(this.CENTERANDSCALE,_28,x,y);
};
this.ResizeTo=function(_29,_2a){
if(this.MapWidth()!=_29||this.MapHeight()!=_2a){
this.ApiCallXY(this.RESIZETO,"",_29,_2a);
}
};
this.Resize=function(){
var d=__$(this.MapID);
if(d.parentNode){
this.ResizeTo(__getDivW(d.parentNode),__getDivH(d.parentNode));
}
};
this.SetZoom=function(_2c){
if(this.GetZoom()!=_2c){
this.ApiCall(this.SETZOOM,_2c);
}
};
this.GetZoom=function(){
return this.GetInt("zoom");
};
this.GetZoomLevelCount=function(){
return this.GetInt("zlct");
};
this.GetMapScale=function(){
return this.GetDbl("scl");
};
this.SetMapScale=function(_2d){
this.ApiCall(this.SETSCALE,_2d);
};
this.ApiCall=function(cmd,arg){
this.ApiCallXY(cmd,arg,"","");
};
this.ApiCallXY=function(cmd,arg,x,y){
if(typeof (arg)=="undefined"){
arg="";
}
this.SetApiCall();
this.SubmitTool(cmd,x,y,arg);
};
this.ttimer=null;
this.OnLoad=function(){
if(this.ttimer==null&&this.GetEnableAnimation()){
this.RunAnimationTimer();
}
};
this.RunAnimationTimer=function(){
if(this.GetAnimationInterval()>0){
clearTimeout(this.ttimer);
this.ttimer=null;
this.ttimer=setTimeout(__getMapToolsName(this.MapID)+".DoAnimation()",this.GetAnimationInterval());
}
};
this.DoAnimation=function(){
if(!__amIsAspAjaxCall){
map_callback(this.UniqueID,__UpdateAnimationLayer,"ANIM",this.MapID,__UpdateAnimationLayerError);
}else{
this.RunAnimationTimer();
}
};
this.RefreshAnimationLayer=function(){
if(!this.GetEnableAnimation()){
this.DoAnimation();
}
return false;
};
this.SetAnimationInterval=function(_34){
this.SetInt("animint",_34);
};
this.GetAnimationInterval=function(){
return this.GetInt("animint");
};
this.GetEnableAnimation=function(){
return this.GetBool("at");
};
this.SetEnableAnimation=function(_35){
if(_35!=this.GetEnableAnimation()){
this.SetBool("at",_35);
if(_35){
this.DoAnimation();
}else{
clearTimeout(this.ttimer);
this.ttimer=null;
}
}
return false;
};
this.hspc=0;
this.vspc=0;
this.MapCanvas=null;
this.MapID="";
this.UniqueID="";
this.ZoomBarVar=null;
this.GetZoomBar=function(){
try{
return eval(this.ZoomBarVar);
}
catch(ex){
}
return null;
};
this.TOOL_ZOOMIN=1;
this.TOOL_ZOOMOUT=2;
this.TOOL_CENTER=3;
this.TOOL_PAN=4;
this.TOOL_INFO=5;
this.TOOL_DISTANCE=6;
this.TOOL_QINFO=7;
this.TOOL_POINT=8;
this.TOOL_LINE=9;
this.TOOL_POLYLINE=10;
this.TOOL_RECT=11;
this.TOOL_CIRCLE=12;
this.TOOL_POLYGON=13;
this.isDragging=false;
this.x1=0;
this.y1=0;
this.x2=0;
this.y2=0;
this.prevPanX=0;
this.prevPanY=0;
this.Xpts=null;
this.Ypts=null;
this.rleft=0;
this.rright=0;
this.rtop=0;
this.rbottom=0;
this.isOnMoveInited=false;
this.qInfoX=0;
this.qInfoY=0;
this.SetTool=function(_36){
this.CancelTool();
this.SetMapTool(_36);
this.UpdateCursor();
};
this.SetArgument=function(arg){
this.SetToolArgument(arg);
};
this.GetArgument=function(){
return this.GetToolArgument();
};
this.GetTool=function(){
return this.GetMapTool();
};
this.UpdateCursor=function(){
this.SetCursor(this.GetCursor());
};
this.StartTool=function(x,y){
this.MapCanvas.setStroke(this.LineWidth());
this.MapCanvas.setColor(this.LineColor());
switch(this.GetTool()){
case this.TOOL_POINT:
this.StartPoint(x,y);
break;
case this.TOOL_ZOOMIN:
case this.TOOL_RECT:
this.StartRect(x,y);
break;
case this.TOOL_LINE:
this.StartLine(x,y);
break;
case this.TOOL_PAN:
this.StartPan(x,y);
break;
case this.TOOL_CIRCLE:
this.StartPolyline(x,y);
this.StartCircle(x,y);
break;
case this.TOOL_DISTANCE:
case this.TOOL_POLYLINE:
this.StartPolyline(x,y);
break;
case this.TOOL_POLYGON:
this.StartPolygon(x,y);
break;
case this.TOOL_ZOOMOUT:
this.StartZoomOut(x,y);
break;
case this.TOOL_CENTER:
this.StartCenter(x,y);
break;
case this.TOOL_INFO:
this.StartInfo(x,y);
break;
case this.TOOL_QINFO:
this.StartQuickInfo(x,y);
break;
}
};
this.DragTool=function(x,y){
switch(this.GetTool()){
case this.TOOL_RECT:
case this.TOOL_ZOOMIN:
this.DrawRect(x,y);
break;
case this.TOOL_LINE:
this.DrawLine(x,y);
break;
case this.TOOL_PAN:
this.DrawPan(x,y);
break;
case this.TOOL_CIRCLE:
this.DrawDistance(x,y,false);
this.DrawCircle(x,y,false);
break;
case this.TOOL_POLYLINE:
this.DrawPolyline(x,y);
break;
case this.TOOL_DISTANCE:
this.DrawDistance(x,y,true);
break;
case this.TOOL_POLYGON:
this.DrawPolygon(x,y);
break;
case this.TOOL_INFO:
this.DrawInfo(x,y);
break;
case this.TOOL_QINFO:
this.DrawInfo(x,y);
break;
}
};
this.StopTool=function(x,y){
switch(this.GetTool()){
case this.TOOL_RECT:
this.StopRect();
break;
case this.TOOL_LINE:
this.StopLine();
break;
case this.TOOL_PAN:
this.StopPan();
break;
case this.TOOL_CIRCLE:
this.StopCircle();
this.StopDistance();
break;
case this.TOOL_ZOOMIN:
this.StopZoomIn();
break;
case this.TOOL_INFO:
this.StopInfo();
break;
case this.TOOL_QINFO:
this.StopQuickInfo();
break;
}
};
this.CancelTool=function(){
this.StopDrag();
this.MapCanvas.clear();
};
this.StopCompTool=function(){
switch(this.GetTool()){
case this.TOOL_POLYLINE:
this.StopPolyline();
break;
case this.TOOL_DISTANCE:
this.StopDistance();
break;
case this.TOOL_POLYGON:
this.StopPolygon();
break;
}
};
this.StartPoint=function(x,y){
this.MapCanvas.clear();
this.OnPointTool(x,y);
};
this.StartZoomOut=function(x,y){
this.MapCanvas.clear();
this.OnZoomOutTool(x,y);
};
this.StartCenter=function(x,y){
this.MapCanvas.clear();
this.OnCenterTool(x,y);
};
this.StartInfo=function(x,y){
this.MapCanvas.clear();
if(this.EnablePan()){
this.StartPan(x,y);
}else{
this.OnInfoTool(x,y);
}
};
this.StartQuickInfo=function(x,y){
this.HideInfoWindow();
this.MapCanvas.clear();
if(this.EnablePan()){
this.StartPan(x,y);
}else{
this.OnQuickInfoTool(x,y);
}
};
this.DrawInfo=function(x,y){
this.UpdateDrag(x,y);
if(Math.abs(this.x1-this.x2)>1||Math.abs(this.y1-this.y2)>1){
this.DrawPan(x,y);
}
};
this.StopInfo=function(){
this.StopDrag();
if(Math.abs(this.x1-this.x2)>1||Math.abs(this.y1-this.y2)>1){
this.SetOverrideTool(this.TOOL_PAN);
this.StopPan();
}else{
this.OnInfoTool(this.x1,this.y1);
}
};
this.StopQuickInfo=function(){
this.StopDrag();
if(Math.abs(this.x1-this.x2)>1||Math.abs(this.y1-this.y2)>1){
this.SetOverrideTool(this.TOOL_PAN);
this.StopPan();
}else{
this.OnQuickInfoTool(this.x1,this.y1);
}
};
this.StopZoomIn=function(){
this.StopDrag();
this.MapCanvas.clear();
this.UpdateRect();
if(this.rright-this.rleft>1){
this.OnZoomInRectTool(this.rleft,this.rtop,this.rright,this.rbottom);
}else{
this.OnZoomInTool(this.x1,this.y1);
}
};
this.StartRect=function(x,y){
this.StartDrag(x,y);
this.UpdateRect();
};
this.DrawRect=function(x,y){
this.MapCanvas.clear();
this.UpdateDrag(x,y);
this.UpdateRect();
this.MapCanvas.drawRect(this.rleft,this.rtop,this.rright-this.rleft,this.rbottom-this.rtop);
this.MapCanvas.paint();
};
this.StopRect=function(){
this.StopDrag();
this.MapCanvas.clear();
this.UpdateRect();
if(this.rright-this.rleft>1){
this.OnRectTool(this.rleft,this.rtop,this.rright,this.rbottom);
}
};
this.UpdateRect=function(){
var _4e=this.x1;
var _4f=this.y1;
if(this.x1>this.x2){
this.rright=this.x1;
this.rleft=this.x2;
}else{
this.rleft=this.x1;
this.rright=this.x2;
}
if(this.y1>this.y2){
this.rbottom=this.y1;
this.rtop=this.y2;
}else{
this.rtop=this.y1;
this.rbottom=this.y2;
}
};
this.StartLine=function(x,y){
this.StartDrag(x,y);
};
this.DrawLine=function(x,y){
this.MapCanvas.clear();
this.UpdateDrag(x,y);
this.MapCanvas.drawLine(this.x1,this.y1,this.x2,this.y2);
this.MapCanvas.paint();
};
this.StopLine=function(){
this.StopDrag();
this.MapCanvas.clear();
if(Math.abs(this.x1-this.x2)>1||Math.abs(this.y1-this.y2)>1){
this.OnLineTool(this.x1,this.y1,this.x2,this.y2);
}
};
this.StartPan=function(x,y){
this.HideInfoWindow();
this.StartDrag(x,y);
this.prevPanX=x;
this.prevPanY=y;
};
this.DrawPan=function(x,y){
this.SetPanCursorForInfo();
this.UpdateDrag(x,y);
try{
var img=__$(this.MapID+"_Image");
var _59=(this.x2-this.x1)+"px";
var top=(this.y2-this.y1)+"px";
img.style.left=_59;
img.style.top=top;
var _5b=0;
while(true){
img=__$(this.MapID+"wms"+_5b);
if(!img){
break;
}
img.style.left=_59;
img.style.top=top;
_5b++;
}
_59=(this.x2-this.x1);
top=(this.y2-this.y1);
this.PanImage(this.MapID+"tile",_59,top);
this.PanBackLayer(x-this.prevPanX,y-this.prevPanY);
this.PanImage(this.MapID+"m",_59,top);
}
catch(e){
}
this.prevPanX=x;
this.prevPanY=y;
};
this.PanImage=function(id,_5d,top){
var _5f=0;
while(true){
img=__$(id+_5f);
if(!img){
break;
}
var _60=img.offsetTop;
var _61=img.offsetLeft;
if(!img.getAttribute("itop")){
img.setAttribute("itop",img.offsetTop);
img.setAttribute("ileft",img.offsetLeft);
}else{
_60=parseInt(img.getAttribute("itop"));
_61=parseInt(img.getAttribute("ileft"));
}
img.style.left=(_61+_5d)+"px";
img.style.top=(_60+top)+"px";
_5f++;
}
};
this.StopPan=function(){
this.UpdateCursor();
this.StopDrag();
if(Math.abs(this.x1-this.x2)>1||Math.abs(this.y1-this.y2)>1){
this.OnLineTool(this.x1,this.y1,this.x2,this.y2);
}else{
if(this.panToolHandler){
try{
this.panToolHandler(this.apiVar,this.GetMouseEventArgs(true));
}
catch(e){
}
}
}
};
this.StartCircle=function(x,y){
this.StartDrag(x,y);
};
this.DrawCircle=function(x,y,_66){
if(_66){
this.MapCanvas.clear();
}
this.UpdateDrag(x,y);
var d=this.dist(this.x1,this.y1,this.x2,this.y2);
this.MapCanvas.drawEllipse(this.x1-d,this.y1-d,d*2,d*2);
this.MapCanvas.paint();
};
this.StopCircle=function(){
this.StopDrag();
this.MapCanvas.clear();
if(Math.abs(this.x1-this.x2)>1){
var d=this.dist(this.x1,this.y1,this.x2,this.y2);
this.OnCircleTool(this.x1,this.y1,d);
}
};
this.dist=function(x1,y1,x2,y2){
return Math.round(Math.abs(this.hypot(x1-x2,y1-y2)));
};
this.hypot=function(a,b){
return Math.sqrt((a*a)+(b*b));
};
this.StartPolyline=function(x,y){
if(!this.isDragging){
this.Xpts=new Array();
this.Ypts=new Array();
}
this.StartDrag(x,y);
this.Xpts[this.Xpts.length]=x;
this.Ypts[this.Ypts.length]=y;
};
this.DrawPolyline=function(x,y){
this.MapCanvas.clear();
this.UpdateDrag(x,y);
this.MapCanvas.drawPolyline(this.Xpts,this.Ypts);
this.MapCanvas.drawLine(this.x1,this.y1,this.x2,this.y2);
this.MapCanvas.paint();
};
this.StopPolyline=function(){
this.StopDrag();
this.MapCanvas.clear();
if(this.Xpts.length>1&&this.dist(this.Xpts[0],this.Ypts[0],this.Xpts[1],this.Ypts[1])>1){
this.OnPolylineTool(this.Xpts,this.Ypts);
}
};
this.DrawDistance=function(x,y,_75){
var _76="#000000";
var _77="#FFFFFF";
this.MapCanvas.clear();
this.UpdateDrag(x,y);
this.MapCanvas.drawPolyline(this.Xpts,this.Ypts);
this.MapCanvas.drawLine(this.x1,this.y1,this.x2,this.y2);
var _78=0;
for(var i=0;i<this.Xpts.length-1;i++){
_78+=this.dist(this.Xpts[i],this.Ypts[i],this.Xpts[i+1],this.Ypts[i+1]);
}
_78+=this.dist(this.x1,this.y1,this.x2,this.y2);
_78*=this.mpp();
var top=this.MapHeight()-28;
if((_78/1609.34)>1){
var _7b=Math.round(1000*_78/1609.34)/1000;
this.MapCanvas.drawString(_7b+" "+this.miles(),1,top,_76,_77);
}else{
_7b=Math.round(_78/0.3048);
this.MapCanvas.drawString(_7b+" "+this.feet(),1,top,_76,_77);
}
if((_78/1000)>1){
_7b=Math.round(1000*_78/1000)/1000;
this.MapCanvas.drawString(_7b+" "+this.km(),1,top+14,_76,_77);
}else{
this.MapCanvas.drawString((Math.round(10*_78)/10)+" "+this.meters(),1,top+14,_76,_77);
}
if(_75){
this.MapCanvas.paint();
}
};
this.StopDistance=function(){
this.StopDrag();
this.MapCanvas.clear();
};
this.StartPolygon=function(x,y){
if(!this.isDragging){
this.Xpts=new Array();
this.Ypts=new Array();
}
this.StartDrag(x,y);
this.Xpts[this.Xpts.length]=x;
this.Ypts[this.Ypts.length]=y;
};
this.DrawPolygon=function(x,y){
this.MapCanvas.clear();
this.UpdateDrag(x,y);
var tx=new Array().concat(this.Xpts);
var ty=new Array().concat(this.Ypts);
tx[tx.length]=this.x2;
ty[ty.length]=this.y2;
this.MapCanvas.drawPolygon(tx,ty);
this.MapCanvas.paint();
};
this.StopPolygon=function(){
this.StopDrag();
this.MapCanvas.clear();
if(this.Xpts.length>2&&this.dist(this.Xpts[0],this.Ypts[0],this.Xpts[1],this.Ypts[1])>1){
this.Xpts[this.Xpts.length]=this.Xpts[0];
this.Ypts[this.Ypts.length]=this.Ypts[0];
this.OnPolygonTool(this.Xpts,this.Ypts);
}
};
this.StartDrag=function(x,y){
this.x1=x;
this.y1=y;
this.x2=this.x1+1;
this.y2=this.y1+1;
this.isDragging=true;
};
this.UpdateDrag=function(x,y){
this.x2=x;
this.y2=y;
};
this.StopDrag=function(){
this.isDragging=false;
return true;
};
this.mouseX=0;
this.mouseY=0;
this.IsLeftDown=function(e){
var _87;
if(e.which==null){
_87=(e.button<2)?1:((e.button==4)?2:3);
}else{
_87=(e.which<2)?1:((e.which==2)?2:3);
}
return (_87==1);
};
this.OnMouseDown=function(e){
if(!this.IsLeftDown(e)){
this.CancelTool();
return;
}
this.OnResize();
this.GetImageXY(e);
if(this.mouseX>=0&&this.mouseX<this.MapWidth()&&this.mouseY>=0&&this.mouseY<this.MapHeight()){
this.StartTool(this.mouseX,this.mouseY);
return false;
}
return false;
};
function CallOnMouseMove(){
var t=_8a;
var W=t.MapWidth();
var H=t.MapHeight();
if(t.mouseX>W){
t.mouseX=W-1;
}
if(t.mouseY>H){
t.mouseY=H-1;
}
if(t.mouseX<=0){
t.mouseX=1;
}
if(t.mouseY<=0){
t.mouseY=1;
}
if(t.isDragging){
t.DragTool(t.mouseX,t.mouseY);
}
return false;
}
var _8d;
var _8e;
var _8f=0;
var _8a=null;
this.OnMouseMove=function(e){
_8a=this;
if(!this.isDragging){
this.RefreshCursor();
this.OnResize();
}
this.GetImageXY(e);
if(this.isDragging){
clearTimeout(_8d);
_8f++;
_8e=function(){
CallOnMouseMove();
};
if(_8f<=3){
_8d=setTimeout(_8e,80);
}else{
_8e();
_8f=0;
}
}
this.ShowLatLong(true);
this.HideToolTips();
return false;
};
this.RefreshCursor=function(){
var _91=__$(this.MapID+"_rf");
if(_91==null){
return;
}
if(_91.value=="1"){
this.UpdateCursor();
_91.value="0";
}
};
this.OnMouseLeave=function(e){
if(this.isDragging&&this.GetMapTool()==this.TOOL_PAN){
this.StopTool();
}
this.ShowLatLong(false);
return false;
};
this.GetCenterLatLong=function(){
return new AspMap.Point(this.GetDbl("_xd"),this.GetDbl("_yd"));
};
this.GetCenterInMapUnits=function(){
return new AspMap.Point(this.GetDbl("_xu"),this.GetDbl("_yu"));
};
this.GetMouseEventArgs=function(_93){
var dpp=this.GetDbl("_dpp");
var upp=this.GetDbl("_upp");
var _96=this.GetCenterLatLong();
var lng=_96.x+(this.mouseX-Math.round(this.MapWidth()/2))*dpp;
var lat=_96.y-(this.mouseY-Math.round(this.MapHeight()/2))*dpp;
_96=this.GetCenterInMapUnits();
var x=_96.x+(this.mouseX-Math.round(this.MapWidth()/2))*upp;
var y=_96.y-(this.mouseY-Math.round(this.MapHeight()/2))*upp;
var _9b=new AspMap.MouseEventArgs();
_9b.x=this.mouseX;
_9b.y=this.mouseY;
_9b.isInside=_93;
_9b.longitude=lng;
_9b.latitude=lat;
_9b.mapPoint=new AspMap.Point(x,y);
return _9b;
};
this.ShowLatLong=function(_9c){
try{
if(this.mouseMoveHandler){
this.mouseMoveHandler(this.apiVar,this.GetMouseEventArgs(_9c));
}
}
catch(ex){
}
};
this.OnMouseOver=function(e){
this.RefreshCursor();
return false;
};
this.OnMouseUp=function(e){
if(this.isDragging){
this.GetImageXY(e);
this.StopTool(this.mouseX,this.mouseY);
}
return false;
};
this.OnDoubleClick=function(e){
var _a0=this.GetTool();
if(this.isDragging){
this.StopCompTool();
}else{
if(_a0==this.TOOL_PAN||_a0==this.TOOL_INFO||_a0==this.TOOL_QINFO){
if(!this.IsLeftDown(e)){
return;
}
this.OnResize();
this.GetImageXY(e);
this.SetOverrideTool(this.TOOL_ZOOMIN);
this.OnZoomInTool(this.mouseX,this.mouseY);
}
}
return false;
};
this.OnKeyDown=function(e){
if(e.keyCode==27){
this.CancelTool();
}
};
this.OnMouseWheel=function(e){
if(this.isDragging||!this.EnableMouseWheel()){
return;
}
this.OnResize();
this.GetImageXY(e);
e=e?e:window.event;
var _a3=e.detail?e.detail*-1:e.wheelDelta/40;
var res=__cancelEvent(e);
this.OnWheel(_a3>0,this.mouseX,this.mouseY);
return res;
};
this.GetImageXY=function(_a5){
if(_a5.pageX){
this.mouseX=_a5.pageX;
this.mouseY=_a5.pageY;
}else{
if(document.documentElement&&document.documentElement.scrollTop){
this.mouseX=_a5.clientX+document.documentElement.scrollLeft-2;
this.mouseY=_a5.clientY+document.documentElement.scrollTop-2;
}else{
this.mouseX=_a5.clientX+document.body.scrollLeft-2;
this.mouseY=_a5.clientY+document.body.scrollTop-2;
}
}
this.mouseX=this.mouseX-this.hspc;
this.mouseY=this.mouseY-this.vspc;
};
this.Init=function(_a6,_a7,_a8){
var _a9=_a6+"_Canvas";
this.MapID=_a6;
this.UniqueID=_a7;
this.ZoomBarVar=_a8;
this.MapCanvas=new jsGraphicsEx(_a9);
this.MapCanvas.setStroke(1);
this.MapCanvas.setColor("#FF0000");
this.MapCanvas.setFont("verdana,geneva,helvetica,sans-serif","12px","font-weight:bold;");
this.UpdateCursor();
};
this.MapWidth=function(){
return parseInt(this.GetMapCanvas().style.width);
};
this.MapHeight=function(){
return parseInt(this.GetMapCanvas().style.height);
};
this.OnResize=function(){
try{
this.vspc=0;
this.hspc=0;
var _aa=this.GetMapCanvas();
do{
this.hspc+=_aa.offsetLeft;
this.vspc+=_aa.offsetTop;
}while((_aa=_aa.offsetParent));
}
catch(ex){
}
};
this.GetMapCanvas=function(){
return __$(this.MapID+"_Canvas");
};
this.GetMapImg=function(){
return __$(this.MapID+"_Image");
};
this.Show=function(){
var div=__$(this.MapID);
if(div){
div.style.visibility="visible";
}
if(this.GetZoomBar()){
this.GetZoomBar().show();
}
this.UpdateBackLayerVisibility();
};
this.Hide=function(){
var div=__$(this.MapID);
if(div){
div.style.visibility="hidden";
}
if(this.GetZoomBar()){
this.GetZoomBar().hide();
}
this.UpdateBackLayerVisibility();
};
this.UpdateMapVisibility=function(){
if(this.IsVisible()){
this.Show();
}else{
this.Hide();
}
};
this.OnPointTool=function(x,y){
var _af=false;
if(this.pointToolHandler){
try{
_af=this.pointToolHandler(this.apiVar,this.GetMouseEventArgs(true));
}
catch(e){
}
}
if(!_af){
this.Submit(x,y,"");
}
};
this.OnRectTool=function(x1,y1,x2,y2){
this.Submit(this.pack(x1,x2),this.pack(y1,y2),"");
};
this.OnLineTool=function(x1,y1,x2,y2){
this.OnRectTool(x1,y1,x2,y2);
};
this.OnCircleTool=function(x,y,_ba){
this.Submit(x,y,_ba);
};
this.OnPolylineTool=function(_bb,_bc){
this.Submit(_bb.join(";"),_bc.join(";"),"");
};
this.OnPolygonTool=function(_bd,_be){
this.Submit(_bd.join(";"),_be.join(";"),"");
};
this.OnZoomInTool=function(x,y){
this.Submit(x,y,"");
};
this.OnZoomInRectTool=function(x1,y1,x2,y2){
this.OnRectTool(x1,y1,x2,y2);
};
this.OnZoomOutTool=function(x,y){
this.Submit(x,y,"");
};
this.OnCenterTool=function(x,y){
this.Submit(x,y,"");
};
this.OnInfoTool=function(x,y){
var _cb=false;
if(this.infoToolHandler){
try{
_cb=this.infoToolHandler(this.apiVar,this.GetMouseEventArgs(true));
}
catch(e){
}
}
if(!_cb){
this.Submit(x,y,"");
}
};
this.OnWheel=function(_cc,x,y){
if(_cc){
var cx=Math.round(this.MapWidth()/2+(x-this.MapWidth()/2)/2);
var cy=Math.round(this.MapHeight()/2+(y-this.MapHeight()/2)/2);
this.AnimateZoom(x,y,_cc);
this.SubmitTool(this.TOOL_ZOOMIN,cx,cy,"");
}else{
var xy=this.Rotate(this.MapWidth()/2,this.MapHeight()/2,x-this.MapWidth()/2,y-this.MapHeight()/2,180);
this.AnimateZoom(x,y,_cc);
this.SubmitTool(this.TOOL_ZOOMOUT,xy.x,xy.y,"");
}
};
this.AnimateZoom=function(x,y,_d4){
var _d5="";
if(_d4){
_d5="aspmap_whzi";
}else{
_d5="aspmap_whzo";
}
var img=__$(_d5);
if(img){
var ext=__getDivExt(this.MapID);
img.style.left=(ext.left+x-29)+"px";
img.style.top=(ext.top+y-29)+"px";
img.src=img.src;
img.style.display="block";
setTimeout("try{__$('"+_d5+"').style.display = 'none';}catch(e){}",320);
}
};
this.Rotate=function(_d8,_d9,x,y,_dc){
_dc=(_dc)*Math.PI/180;
var px=Math.round(x*Math.cos(_dc)-y*Math.sin(_dc)+_d8);
var py=Math.round(x*Math.sin(_dc)+y*Math.cos(_dc)+_d9);
return {x:px,y:py};
};
this.OnQuickInfoTool=function(x,y){
this.SetCmdParams(this.GetTool(),this.GetArgument(),x,y,"");
this.qInfoX=x;
this.qInfoY=y;
map_callback(this.UniqueID,__showQuickInfo,"QINFO",this);
this.SetCmdParams(this.GetTool(),this.GetArgument(),"","","");
};
this.HotspotInfoClick=function(_e1,xy){
var _e3=xy.split(",");
this.qInfoX=parseInt(_e3[0]);
this.qInfoY=parseInt(_e3[1]);
map_callback(this.UniqueID,__showQuickInfo,"HINFO"+_e1,this);
};
this.MarkerClick=function(_e4,_e5,_e6){
var _e7=false;
if(this.markerClickHandler){
try{
var _e8=new AspMap.MarkerClickEventArgs();
_e8.argument=_e4;
_e8.content="";
var _e9=__$(this.MapID+"c"+_e5);
if(_e9){
_e8.content=_e9.innerHTML;
}
_e7=this.markerClickHandler(this.apiVar,_e8);
}
catch(e){
}
}
if(!_e7&&_e6.length>0){
eval(_e6);
}
};
this.MarkerInfo=function(_ea,x,y){
this.qInfoX=parseInt(x);
this.qInfoY=parseInt(y);
var _ed=__$(this.MapID+"c"+_ea);
if(_ed){
this.ShowInfo(_ed.innerHTML);
}
};
this.HideInfoWindow=function(){
__hideQuickInfo(this.MapID);
};
this.ShowInfo=function(_ee){
if(_ee.length==0){
return;
}
var _ef=__$(this.MapID+"_infodiv");
if(!_ef){
return;
}
var _f0=__$(this.MapID+"_infoct");
if(!_f0){
return;
}
var _f1=__$(this.MapID+"_infodivc");
if(!_f1){
return;
}
var _f2=__$("iwui$");
if(!_f2){
return;
}
__hideQuickInfo(this.MapID);
_f1.innerHTML=_f2.innerHTML.replace("iwctnt",_ee);
var x=this.qInfoX;
var y=this.qInfoY;
var _f5=__getDivW(_ef);
var _f6=__getDivH(_ef);
var _f7=this.MapWidth();
var _f8=this.MapHeight();
var _f9=12;
var _fa=20;
var _fb=20;
var _fc=0;
var _fd=0;
if(_f5<=_f7){
_fc=x-_f5/2-_fa/2;
if(_fc+_f5>_f7){
var d=_fc+_f5-_f7+1;
if(_f7-x<=_f9){
d-=_f9-(_f7-x)+1;
}
_fc-=d;
}
if(_fc<=0){
var d=Math.abs(_fc);
if(x<=_fa+_f9){
d-=_fa+_f9-x;
}else{
d+=1;
}
_fc+=d;
}
}else{
_fc=x-_f5/2-_fa/2;
if(_fc<=0){
var d=Math.abs(_fc);
if(x<_fa+_f9){
d-=_fa+_f9-x;
}else{
d+=1;
}
_fc+=d;
}else{
_fc=1;
}
}
x-=_fa;
if(((y+_f6+_fb>=_f8)||(y-_f6-_fb>0))&&(y-_f6-_fb)>=0){
_fd=y-_f6-_fb;
y-=22;
}else{
_fd=y+_fb;
}
if(_fd>y){
_f0.src=__$("rtct$").src;
}else{
_f0.src=__$("rbct$").src;
}
_ef.style.left=_fc+"px";
_ef.style.top=_fd+"px";
_f0.style.left=x+"px";
_f0.style.top=y+"px";
_f0.style.visibility="visible";
_ef.style.visibility="visible";
};
this.Submit=function(x,y,arg){
this.HideInfoWindow();
this.SetCmdParams(this.GetTool(),this.GetArgument(),x,y,arg);
__submitMapToolCmd();
};
this.SubmitTool=function(tool,x,y,arg){
this.HideInfoWindow();
this.SetCmdParams(this.GetTool(),this.GetArgument(),x,y,arg);
this.SetOverrideTool(tool);
__submitMapToolCmd();
};
this.SetCmdParams=function(tool,targ,x,y,arg){
__$(this.MapID+"_arg").value=arg;
__$(this.MapID+"_targ").value=targ;
__$(this.MapID+"_tool").value=tool;
__$(this.MapID+"_x").value=x;
__$(this.MapID+"_y").value=y;
};
this.SetInfoCursor=function(){
this.GetMapCanvas().style.cursor="help";
};
this.SetCrossCursor=function(){
this.GetMapCanvas().style.cursor="crosshair";
};
this.SetPanCursor=function(){
this.GetMapCanvas().style.cursor="move";
};
this.SetPanCursorForInfo=function(){
var c=this.GetStr("_curspan");
if(c.length>0){
this.GetMapCanvas().style.cursor=c;
}
};
this.SetDefCursor=function(){
this.GetMapCanvas().style.cursor="default";
};
this.GetCursor=function(){
return __$(this.MapID+"_curs").value;
};
this.SetCursor=function(c){
if(c==null){
return;
}
__$(this.MapID+"_curs").value=c;
if(c.length>0){
this.GetMapCanvas().style.cursor=c;
return;
}
var tool=this.GetTool();
if(tool==this.TOOL_CENTER){
this.SetCrossCursor();
}else{
if(tool==this.TOOL_PAN){
this.SetPanCursor();
}else{
if(tool==this.TOOL_INFO||tool==this.TOOL_QINFO){
this.SetInfoCursor();
}else{
this.SetDefCursor();
}
}
}
};
this.HideToolTips=function(){
__TT.hideToolpipsFromTracking();
};
this.GetMapTool=function(){
return parseInt(__$(this.MapID+"_tool").value);
};
this.SetMapTool=function(tool){
__$(this.MapID+"_tool").value=tool;
};
this.SetOverrideTool=function(tool){
__$(this.MapID+"_otool").value=tool;
};
this.SetApiCall=function(){
__$(this.MapID+"_api").value="1";
};
this.SetToolArgument=function(arg){
__$(this.MapID+"_targ").value=arg;
};
this.GetToolArgument=function(){
return __$(this.MapID+"_targ").value;
};
this.mpp=function(){
return parseFloat(__$(this.MapID+"_mpp").value);
};
this.miles=function(){
return __$(this.MapID+"_mis").value;
};
this.feet=function(){
return __$(this.MapID+"_fts").value;
};
this.km=function(){
return __$(this.MapID+"_kms").value;
};
this.meters=function(){
return __$(this.MapID+"_mts").value;
};
this.LineWidth=function(){
return parseInt(__$(this.MapID+"_tlw").value);
};
this.LineColor=function(){
return __$(this.MapID+"_tlc").value;
};
this.EnablePan=function(){
return __$(this.MapID+"_enpan").value=="1";
};
this.EnableMouseWheel=function(){
return __$(this.MapID+"_emw").value=="1";
};
this.BackLayerService=function(){
var s=__$(this.MapID+"blservice");
if(s){
return s.value;
}
return null;
};
this.BackLayerType=function(){
var s=__$(this.MapID+"bltype");
if(s){
return parseInt(s.value);
}
return 0;
};
this.BackLayerVisible=function(){
var s=__$(this.MapID+"blvisible");
if(s){
return s.value=="1";
}
return false;
};
this.IsVisible=function(){
return this.GetBool("_vis");
};
this.SetVisible=function(_114){
this.SetBool("_vis",_114);
this.UpdateMapVisibility();
};
this.GetDbl=function(_115){
return parseFloat(this.GetStr(_115));
};
this.SetDbl=function(_116,_117){
this.SetStr(_116,_117);
};
this.GetBool=function(_118){
return this.GetStr(_118)=="1";
};
this.SetBool=function(_119,_11a){
this.SetStr(_119,_11a?"1":"0");
};
this.GetInt=function(_11b){
return parseInt(this.GetStr(_11b));
};
this.SetInt=function(_11c,_11d){
this.SetStr(_11c,Math.round(_11d));
};
this.GetStr=function(_11e){
return __$(this.MapID+_11e).value;
};
this.SetStr=function(_11f,_120){
__$(this.MapID+_11f).value=_120;
};
this.pack=function(a,b){
return (a+";"+b);
};
this.Print=function(){
var _123=null;
try{
var html=__$(this.MapID).innerHTML;
var _125=/[o]n[a-zA-Z0-9]+="[^"]*"/g;
html=html.replace(_125,"");
var _126=frames["amprnfr"];
_123=_126.document.getElementById("content");
_123.style.position="relative";
_123.style.width=this.MapWidth()+"px";
_123.style.height=this.MapHeight()+"px";
_123.innerHTML=html;
var _127=_126.document.getElementById(this.MapID+"_Canvas");
_127.style.backgroundImage="";
var _128=__$(this.MapID+"bglayer").cloneNode(true);
_128.style.left="0px";
_128.style.top="0px";
_128.style.zIndex=0;
_123.appendChild(_128);
_126.document.body.style.visibility="visible";
_126.focus();
_126.prn();
}
catch(e){
}
};
this.backLayerDiv=null;
this.backLayer=null;
this.InitBackLayerDiv=function(){
var _129=document.createElement("div");
_129.style.position="absolute";
_129.id=this.MapID+"bglayer";
this.InitBackLayerSize(_129);
_129.style.zIndex="180";
_129.style.visibility="hidden";
document.body.appendChild(_129);
this.backLayerDiv=_129;
var _12a=this.BackLayerService();
if(_12a){
if(_12a=="google"){
this.backLayer=new AspMapGM(this.backLayerDiv,this.BackLayerType());
}else{
if(_12a=="ve"){
this.backLayer=new AspMapVE(this.backLayerDiv,this.BackLayerType());
}
}
if(this.backLayer.IsValid()){
this.backLayer.CenterAndZoom(this.GetCenterLatLong(),this.GetZoom());
if(this.BackLayerVisible()&&this.IsVisible()){
_129.style.visibility="visible";
}
}else{
this.backLayer=null;
}
}
};
this.UpdateBackLayer=function(){
if(this.backLayer){
this.UpdateBackLayerVisibility();
this.backLayer.SetMapType(this.BackLayerType());
this.backLayer.CenterAndZoom(this.GetCenterLatLong(),this.GetZoom());
}
};
this.UpdateBackLayerVisibility=function(){
if(this.backLayer){
if(this.BackLayerVisible()&&this.IsVisible()){
this.backLayerDiv.style.visibility="visible";
}else{
this.backLayerDiv.style.visibility="hidden";
}
}
};
this.PanBackLayer=function(dx,dy){
if(this.backLayer){
this.backLayer.Pan(dx,dy);
}
};
this.ResizeBackLayer=function(){
if(this.backLayerDiv){
this.InitBackLayerSize(this.backLayerDiv);
if(this.backLayer){
this.backLayer.Resize(this.MapWidth(),this.MapHeight());
}
}
};
this.InitBackLayerSize=function(div){
var ext=__getDivExt(this.MapID);
if(ext==null){
return;
}
div.style.left=ext.left+"px";
div.style.top=ext.top+"px";
div.style.width=this.MapWidth()+"px";
div.style.height=this.MapHeight()+"px";
};
this.load=function(){
this.RegisterAspAjaxHandlers();
this.InitBackLayerDiv();
this.UpdateMapVisibility();
};
this.pageLoaded=function(_12f,args){
this.UpdateMapVisibility();
this.ResizeBackLayer();
this.UpdateBackLayer();
};
this.resize=function(e){
this.ResizeBackLayer();
};
__addHandlerCtx(window,"load",this);
__addHandlerCtx(window,"resize",this);
this.RegisterAspAjaxHandlers=function(){
try{
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(__getAjaxEventCtx(this,"pageLoaded"));
}
catch(ex){
}
};
}
__addHandler(window,"load",__amAspAjaxInit);
var __amIsAspAjaxCall=false;
var __amEventValidation=null;
function __amAspAjaxInit(){
try{
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(__amAspAjaxBeginRequest);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(__amAspAjaxEndRequest);
}
catch(ex){
}
}
function __amAspAjaxBeginRequest(_132,args){
__amIsAspAjaxCall=true;
try{
__TT.H();
}
catch(e){
}
}
function __amAspAjaxEndRequest(_134,args){
__amIsAspAjaxCall=false;
try{
var ev=__$("__EVENTVALIDATION");
if(ev){
__amEventValidation=ev.value;
}
}
catch(ex){
}
}
function __amUpdateEventValidation(){
try{
if(__amEventValidation){
var ev=__$("__EVENTVALIDATION");
if(ev){
ev.value=__amEventValidation;
}
}
}
catch(ex){
}
}
function AspMapGM(div,_139){
this.mapObj=null;
this.dragObj=null;
this.IsValid=function(){
return (this.mapObj!=null);
};
this.CenterAndZoom=function(_13a,zoom){
if(!this.IsValid()){
return;
}
try{
this.mapObj.setCenter(new GLatLng(_13a.y,_13a.x),zoom);
}
catch(e){
}
};
this.Pan=function(dx,dy){
if(!this.IsValid()){
return;
}
try{
this.dragObj.moveBy(new GSize(dx,dy));
}
catch(e){
}
};
this.SetMapType=function(type){
if(!this.IsValid()){
return;
}
try{
switch(type){
case 0:
this.mapObj.setMapType(G_NORMAL_MAP);
break;
case 1:
this.mapObj.setMapType(G_SATELLITE_MAP);
break;
case 2:
this.mapObj.setMapType(G_HYBRID_MAP);
break;
case 3:
this.mapObj.setMapType(G_PHYSICAL_MAP);
break;
}
}
catch(e){
}
};
this.Resize=function(_13f,_140){
if(!this.IsValid()){
return;
}
try{
this.mapObj.checkResize();
}
catch(e){
}
};
try{
this.mapObj=new GMap2(div);
this.SetMapType(_139);
if(typeof this.mapObj.getDragObject=="function"){
this.dragObj=this.mapObj.getDragObject();
}
}
catch(e){
this.mapObj=null;
}
}
function AspMapVE(div,_142){
this.mapObj=null;
this.dragObj=null;
this.IsValid=function(){
return (this.mapObj!=null);
};
this.CenterAndZoom=function(_143,zoom){
if(!this.IsValid()){
return;
}
try{
this.mapObj.SetCenterAndZoom(new VELatLong(_143.y,_143.x),zoom+1);
}
catch(e){
}
};
this.Pan=function(dx,dy){
if(!this.IsValid()){
return;
}
try{
this.mapObj.vemapcontrol.PanMap(-dx,-dy);
}
catch(e){
}
};
this.SetMapType=function(type){
if(!this.IsValid()){
return;
}
try{
switch(type){
case 0:
this.mapObj.SetMapStyle(VEMapStyle.Road);
break;
case 1:
this.mapObj.SetMapStyle(VEMapStyle.Aerial);
break;
case 2:
this.mapObj.SetMapStyle(VEMapStyle.Hybrid);
break;
}
}
catch(e){
}
};
this.Resize=function(_148,_149){
if(!this.IsValid()){
return;
}
try{
this.mapObj.Resize(_148,_149);
}
catch(e){
}
};
try{
this.mapObj=new VEMap(div.id);
if(this.mapObj!=null){
try{
this.mapObj.LoadMap(null,null,null,true);
this.mapObj.AttachEvent("onmousedown",function(){
return true;
});
}
catch(e){
}
this.SetMapType(_142);
this.mapObj.HideDashboard();
}
}
catch(e){
this.mapObj=null;
}
}
function AspMapZoomBar(_14a,_14b,_14c,_14d,_14e,pos){
this.mapID=_14b;
this.clientID=_14a;
this.showBar=_14e;
this.levelCount=_14c;
this.levelH=_14d;
this.drag=false;
this.pos=pos;
this.mousedown=function(e){
if(!this.showBar){
return true;
}
if(e.preventDefault){
e.preventDefault();
}
this.drag=true;
return false;
};
this.mousemove=function(e){
if(!this.drag||!this.showBar){
return true;
}
this.moveThumb(e);
return false;
};
this.mouseup=function(e){
if(!this.drag||!this.showBar){
return true;
}
this.drag=false;
var _153=this.moveThumb(e);
if(_153>=0){
var map=__getMapTools(this.mapID);
if(!map){
return false;
}
map.SetZoom(_153);
}
return false;
};
this.resize=function(e){
if(this.drag){
return true;
}
if(this.pos==0){
return true;
}
var ext=__getDivExt(this.mapID);
if(!ext){
return true;
}
var div=__$(this.clientID);
div.style.top=ext.top+4+"px";
if(this.pos==1){
div.style.left=ext.left+4+"px";
}else{
div.style.left=ext.right-__getDivW(div)-4+"px";
}
var map=__getMapTools(this.mapID);
if(map&&map.IsVisible()){
this.show();
}
return true;
};
this.show=function(){
var div=__$(this.clientID);
if(div){
div.style.visibility="visible";
}
};
this.hide=function(){
var div=__$(this.clientID);
if(div){
div.style.visibility="hidden";
}
};
this.load=function(e){
this.resize();
return true;
};
this.moveThumb=function(e){
e=e||window.event;
var _15d=this.getMouseY(this.barID(),e);
var barH=__getDivH(__$(this.barID()));
var _15f=0;
if(_15d<=0){
_15f=this.levelCount-1;
}else{
if(_15d>=barH){
_15f=0;
}else{
_15f=this.mouseToLevel(_15d);
}
}
if(_15f<0||_15f>=this.levelCount){
return -1;
}
this.updateThumb(_15f);
return _15f;
};
this.updateThumb=function(_160){
if(_160<0||_160>=this.levelCount){
return;
}
var _161=__$(this.thumbID());
if(!_161){
return;
}
var _162=__getDivH(_161);
_161.style.top=(this.levelCount-_160-1)*this.levelH-Math.round((_162-this.levelH)/2)+"px";
};
this.mouseToLevel=function(_163){
return this.levelCount-Math.floor(_163/this.levelH)-1;
};
this.getMouseY=function(_164,ev){
var _166=__$(_164);
if(!_166){
return 0;
}
var top=0,y=0;
while(_166.offsetParent){
top+=_166.offsetTop;
_166=_166.offsetParent;
}
top+=_166.offsetTop;
var _169=__$(this.clientID);
top+=Math.round((_169.offsetHeight-_169.clientHeight)/2);
ev=ev||window.event;
if(ev.pageY){
y=ev.pageY;
}else{
if(document.documentElement&&document.documentElement.scrollTop){
y=ev.clientY+document.documentElement.scrollTop-document.documentElement.clientTop;
}else{
y=ev.clientY+document.body.scrollTop-document.body.clientTop;
}
}
return y-top;
};
this.thumbID=function(){
return this.clientID+"_th";
};
this.barID=function(){
return this.clientID+"_bar";
};
this.pageLoaded=function(_16a,args){
var _16c=__getMapTools(this.mapID);
if(_16c==null){
return;
}
this.resize();
this.updateThumb(_16c.GetZoom());
};
if(this.showBar){
__addHandlerCtx(document.body,"mousemove",this);
__addHandlerCtx(document.body,"mouseup",this);
__addHandlerCtx(__$(this.thumbID()),"mousedown",this);
}
if(this.pos>0){
__addHandlerCtx(window,"load",this);
__addHandlerCtx(window,"resize",this);
this.resize(null);
}
try{
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(__getAjaxEventCtx(this,"pageLoaded"));
}
catch(ex){
}
}
function __selectMapTool(_16d,_16e,_16f,arg){
if(_16f==null){
return;
}
var _171=__$(_16d+"_tbutt");
if(_171==null){
return;
}
var _172=__$(_171.value);
var _173=__getMapTools(_16d);
if(_173==null){
return;
}
_173.SetTool(_16e);
_173.SetArgument(arg);
var attr=_16f.getAttribute("curs");
if(attr!=null){
_173.SetCursor(attr);
}
if(_172){
var attr=_172.getAttribute("normimg");
if(attr!=null){
_172.src=attr;
}
attr=_172.getAttribute("normbord");
if(attr!="notset"){
_172.style.borderStyle=attr;
}
}
attr=_16f.getAttribute("selimg");
if(attr!=null){
_16f.src=attr;
}
attr=_16f.getAttribute("selbord");
if(attr!="notset"){
_16f.style.borderStyle=attr;
}
_171.value=_16f.id;
}
function __markerClick(_175,_176,_177,_178){
var map=__getMapTools(_175);
if(map){
map.MarkerClick(_176,_177,_178);
}
}
function __markerInfo(_17a,_17b,x,y){
var map=__getMapTools(_17a);
if(map){
map.MarkerInfo(_17b,x,y);
}
}
function __hsInfo(_17f,_180,xy){
var map=__getMapTools(_17f);
if(map){
map.HotspotInfoClick(_180,xy);
}
}
function __showQuickInfo(_183,_184){
if(!_183||_183.length==0||!_184){
return;
}
_184.ShowInfo(_183);
}
function __hideQuickInfo(_185){
try{
__$(_185+"_infodiv").style.visibility="hidden";
__$(_185+"_infoct").style.visibility="hidden";
}
catch(e){
}
}
function __$(id){
if(id==null||id.length==0){
return null;
}
var elem=(document.getElementById?document.getElementById(id):document.all?document.all[id]:null);
if(elem==null){
var _188=document.getElementsByName(id);
if(_188!=null){
elem=_188[0];
}
}
return elem;
}
function __getDivExt(_189){
var _18a=__$(_189);
if(!_18a){
return null;
}
var top=0,left=0;
var div=_18a;
while(_18a.offsetParent){
top+=_18a.offsetTop;
left+=_18a.offsetLeft;
_18a=_18a.offsetParent;
}
top+=_18a.offsetTop;
left+=_18a.offsetLeft;
return {left:left,top:top,right:left+__getDivW(div),bottom:top+__getDivH(div)};
}
function __getDivW(el){
return (el?(el.offsetWidth||el.style.pixelWidth||0):0);
}
function __getDivH(el){
return (el?(el.offsetHeight||el.style.pixelHeight||0):0);
}
function __clearDiv(elem){
if(elem){
while(elem.hasChildNodes()){
elem.removeChild(elem.firstChild);
}
elem.innerHTML="";
}
}
function __cancelEvent(e){
e=e?e:window.event;
if(e.stopPropagation){
e.stopPropagation();
}
if(e.preventDefault){
e.preventDefault();
}
e.cancelBubble=true;
e.cancel=true;
e.returnValue=false;
return false;
}
function __MWFF(_192,_193){
var el=__$(_192);
if(!el){
return;
}
if(el.addEventListener){
el.addEventListener("DOMMouseScroll",_193,false);
}
}
function __addHandler(el,_196,_197){
if(!el){
return;
}
if(el.addEventListener){
el.addEventListener(_196,_197,false);
}else{
if(el.attachEvent){
el.attachEvent("on"+_196,_197);
}else{
el["on"+_196]=_197;
}
}
}
function __addHandlerCtx(el,_199,_19a){
var _19b=__getHandlerCtx(_19a,_199);
__addHandler(el,_199,_19b);
}
function __getHandlerCtx(_19c,_19d){
return (function(e){
return _19c[_19d](e);
});
}
function __getAjaxEventCtx(_19f,_1a0){
return (function(_1a1,args){
return _19f[_1a0](_1a1,args);
});
}
function __getMapTools(_1a3){
var _1a4=null;
try{
_1a4=eval(_1a3.toLowerCase()+"_tools");
}
catch(ex){
}
return _1a4;
}
function __getMapToolsName(_1a5){
return _1a5.toLowerCase()+"_tools";
}
var jg_ok,jg_ie,jg_fast,jg_dom,jg_moz;
function chkDHTM(x,i){
x=document.body||null;
jg_ie=x&&typeof x.insertAdjacentHTML!="undefined"&&document.createElement;
jg_dom=(x&&!jg_ie&&typeof x.appendChild!="undefined"&&typeof document.createRange!="undefined"&&typeof (i=document.createRange()).setStartBefore!="undefined"&&typeof i.createContextualFragment!="undefined");
jg_fast=jg_ie&&document.all&&!window.opera;
jg_moz=jg_dom&&typeof x.style.MozOpacity!="undefined";
jg_ok=!!(jg_ie||jg_dom);
}
function pntCnvDom(){
var x=this.wnd.document.createRange();
var _1a9=this.cnv();
x.setStartBefore(_1a9);
x=x.createContextualFragment(jg_fast?this.htmRpc():this.htm);
if(_1a9){
_1a9.appendChild(x);
}
this.htm="";
}
function pntCnvIe(){
var _1aa=this.cnv();
if(_1aa){
_1aa.insertAdjacentHTML("BeforeEnd",jg_fast?this.htmRpc():this.htm);
}
this.htm="";
}
function pntDoc(){
this.wnd.document.write(jg_fast?this.htmRpc():this.htm);
this.htm="";
}
function pntN(){
}
function mkDiv(x,y,w,h){
this.htm+="<div style=\"position:absolute;"+"left:"+x+"px;"+"top:"+y+"px;"+"width:"+w+"px;"+"height:"+h+"px;"+"clip:rect(0,"+w+"px,"+h+"px,0);"+"background-color:"+this.color+(!jg_moz?";overflow:hidden":"")+";\"></div>";
}
function mkDivIe(x,y,w,h){
this.htm+="%%"+this.color+";"+x+";"+y+";"+w+";"+h+";";
}
function mkDivPrt(x,y,w,h){
this.htm+="<div style=\"position:absolute;"+"border-left:"+w+"px solid "+this.color+";"+"left:"+x+"px;"+"top:"+y+"px;"+"width:0px;"+"height:"+h+"px;"+"clip:rect(0,"+w+"px,"+h+"px,0);"+"background-color:"+this.color+(!jg_moz?";overflow:hidden":"")+";\"></div>";
}
var regex=/%%([^;]+);([^;]+);([^;]+);([^;]+);([^;]+);/g;
function htmRpc(){
return this.htm.replace(regex,"<div style=\"overflow:hidden;position:absolute;background-color:"+"$1;left:$2px;top:$3px;width:$4px;height:$5px\"></div>\n");
}
function htmPrtRpc(){
return this.htm.replace(regex,"<div style=\"overflow:hidden;position:absolute;background-color:"+"$1;left:$2px;top:$3px;width:$4px;height:$5px;border-left:$4px solid $1\"></div>\n");
}
function mkLin(x1,y1,x2,y2){
if(x1>x2){
var _x2=x2;
var _y2=y2;
x2=x1;
y2=y1;
x1=_x2;
y1=_y2;
}
var dx=x2-x1,dy=Math.abs(y2-y1),x=x1,y=y1,_1c1=(y1>y2)?-1:1;
if(dx>=dy){
var pr=dy<<1,pru=pr-(dx<<1),p=pr-dx,ox=x;
while(dx>0){
--dx;
++x;
if(p>0){
this.mkDiv(ox,y,x-ox,1);
y+=_1c1;
p+=pru;
ox=x;
}else{
p+=pr;
}
}
this.mkDiv(ox,y,x2-ox+1,1);
}else{
var pr=dx<<1,pru=pr-(dy<<1),p=pr-dy,oy=y;
if(y2<=y1){
while(dy>0){
--dy;
if(p>0){
this.mkDiv(x++,y,1,oy-y+1);
y+=_1c1;
p+=pru;
oy=y;
}else{
y+=_1c1;
p+=pr;
}
}
this.mkDiv(x2,y2,1,oy-y2+1);
}else{
while(dy>0){
--dy;
y+=_1c1;
if(p>0){
this.mkDiv(x++,oy,1,y-oy);
p+=pru;
oy=y;
}else{
p+=pr;
}
}
this.mkDiv(x2,oy,1,y2-oy+1);
}
}
}
function mkLin2D(x1,y1,x2,y2){
if(x1>x2){
var _x2=x2;
var _y2=y2;
x2=x1;
y2=y1;
x1=_x2;
y1=_y2;
}
var dx=x2-x1,dy=Math.abs(y2-y1),x=x1,y=y1,_1d1=(y1>y2)?-1:1;
var s=this.stroke;
if(dx>=dy){
if(dx>0&&s-3>0){
var _s=(s*dx*Math.sqrt(1+dy*dy/(dx*dx))-dx-(s>>1)*dy)/dx;
_s=(!(s-4)?Math.ceil(_s):Math.round(_s))+1;
}else{
var _s=s;
}
var ad=Math.ceil(s/2);
var pr=dy<<1,pru=pr-(dx<<1),p=pr-dx,ox=x;
while(dx>0){
--dx;
++x;
if(p>0){
this.mkDiv(ox,y,x-ox+ad,_s);
y+=_1d1;
p+=pru;
ox=x;
}else{
p+=pr;
}
}
this.mkDiv(ox,y,x2-ox+ad+1,_s);
}else{
if(s-3>0){
var _s=(s*dy*Math.sqrt(1+dx*dx/(dy*dy))-(s>>1)*dx-dy)/dy;
_s=(!(s-4)?Math.ceil(_s):Math.round(_s))+1;
}else{
var _s=s;
}
var ad=Math.round(s/2);
var pr=dx<<1,pru=pr-(dy<<1),p=pr-dy,oy=y;
if(y2<=y1){
++ad;
while(dy>0){
--dy;
if(p>0){
this.mkDiv(x++,y,_s,oy-y+ad);
y+=_1d1;
p+=pru;
oy=y;
}else{
y+=_1d1;
p+=pr;
}
}
this.mkDiv(x2,y2,_s,oy-y2+ad);
}else{
while(dy>0){
--dy;
y+=_1d1;
if(p>0){
this.mkDiv(x++,oy,_s,y-oy+ad);
p+=pru;
oy=y;
}else{
p+=pr;
}
}
this.mkDiv(x2,oy,_s,y2-oy+ad+1);
}
}
}
function mkOv(left,top,_1dc,_1dd){
var a=(++_1dc)>>1,b=(++_1dd)>>1,wod=_1dc&1,hod=_1dd&1,cx=left+a,cy=top+b,x=0,y=b,ox=0,oy=b,aa2=(a*a)<<1,aa4=aa2<<1,bb2=(b*b)<<1,bb4=bb2<<1,st=(aa2>>1)*(1-(b<<1))+bb2,tt=(bb2>>1)-aa2*((b<<1)-1),w,h;
while(y>0){
if(st<0){
st+=bb2*((x<<1)+3);
tt+=bb4*(++x);
}else{
if(tt<0){
st+=bb2*((x<<1)+3)-aa4*(y-1);
tt+=bb4*(++x)-aa2*(((y--)<<1)-3);
w=x-ox;
h=oy-y;
if((w&2)&&(h&2)){
this.mkOvQds(cx,cy,x-2,y+2,1,1,wod,hod);
this.mkOvQds(cx,cy,x-1,y+1,1,1,wod,hod);
}else{
this.mkOvQds(cx,cy,x-1,oy,w,h,wod,hod);
}
ox=x;
oy=y;
}else{
tt-=aa2*((y<<1)-3);
st-=aa4*(--y);
}
}
}
w=a-ox+1;
h=(oy<<1)+hod;
y=cy-oy;
this.mkDiv(cx-a,y,w,h);
this.mkDiv(cx+ox+wod-1,y,w,h);
}
function mkOv2D(left,top,_1f2,_1f3){
var s=this.stroke;
_1f2+=s+1;
_1f3+=s+1;
var a=_1f2>>1,b=_1f3>>1,wod=_1f2&1,hod=_1f3&1,cx=left+a,cy=top+b,x=0,y=b,aa2=(a*a)<<1,aa4=aa2<<1,bb2=(b*b)<<1,bb4=bb2<<1,st=(aa2>>1)*(1-(b<<1))+bb2,tt=(bb2>>1)-aa2*((b<<1)-1);
if(s-4<0&&(!(s-2)||_1f2-51>0&&_1f3-51>0)){
var ox=0,oy=b,w,h,pxw;
while(y>0){
if(st<0){
st+=bb2*((x<<1)+3);
tt+=bb4*(++x);
}else{
if(tt<0){
st+=bb2*((x<<1)+3)-aa4*(y-1);
tt+=bb4*(++x)-aa2*(((y--)<<1)-3);
w=x-ox;
h=oy-y;
if(w-1){
pxw=w+1+(s&1);
h=s;
}else{
if(h-1){
pxw=s;
h+=1+(s&1);
}else{
pxw=h=s;
}
}
this.mkOvQds(cx,cy,x-1,oy,pxw,h,wod,hod);
ox=x;
oy=y;
}else{
tt-=aa2*((y<<1)-3);
st-=aa4*(--y);
}
}
}
this.mkDiv(cx-a,cy-oy,s,(oy<<1)+hod);
this.mkDiv(cx+a+wod-s,cy-oy,s,(oy<<1)+hod);
}else{
var _a=(_1f2-(s<<1))>>1,_b=(_1f3-(s<<1))>>1,_x=0,_y=_b,_aa2=(_a*_a)<<1,_aa4=_aa2<<1,_bb2=(_b*_b)<<1,_bb4=_bb2<<1,_st=(_aa2>>1)*(1-(_b<<1))+_bb2,_tt=(_bb2>>1)-_aa2*((_b<<1)-1),pxl=new Array(),pxt=new Array(),_pxb=new Array();
pxl[0]=0;
pxt[0]=b;
_pxb[0]=_b-1;
while(y>0){
if(st<0){
pxl[pxl.length]=x;
pxt[pxt.length]=y;
st+=bb2*((x<<1)+3);
tt+=bb4*(++x);
}else{
if(tt<0){
pxl[pxl.length]=x;
st+=bb2*((x<<1)+3)-aa4*(y-1);
tt+=bb4*(++x)-aa2*(((y--)<<1)-3);
pxt[pxt.length]=y;
}else{
tt-=aa2*((y<<1)-3);
st-=aa4*(--y);
}
}
if(_y>0){
if(_st<0){
_st+=_bb2*((_x<<1)+3);
_tt+=_bb4*(++_x);
_pxb[_pxb.length]=_y-1;
}else{
if(_tt<0){
_st+=_bb2*((_x<<1)+3)-_aa4*(_y-1);
_tt+=_bb4*(++_x)-_aa2*(((_y--)<<1)-3);
_pxb[_pxb.length]=_y-1;
}else{
_tt-=_aa2*((_y<<1)-3);
_st-=_aa4*(--_y);
_pxb[_pxb.length-1]--;
}
}
}
}
var ox=-wod,oy=b,_oy=_pxb[0],l=pxl.length,w,h;
for(var i=0;i<l;i++){
if(typeof _pxb[i]!="undefined"){
if(_pxb[i]<_oy||pxt[i]<oy){
x=pxl[i];
this.mkOvQds(cx,cy,x,oy,x-ox,oy-_oy,wod,hod);
ox=x;
oy=pxt[i];
_oy=_pxb[i];
}
}else{
x=pxl[i];
this.mkDiv(cx-x,cy-oy,1,(oy<<1)+hod);
this.mkDiv(cx+ox+wod,cy-oy,1,(oy<<1)+hod);
ox=x;
oy=pxt[i];
}
}
this.mkDiv(cx-a,cy-oy,1,(oy<<1)+hod);
this.mkDiv(cx+ox+wod,cy-oy,1,(oy<<1)+hod);
}
}
function mkRect(x,y,w,h){
var s=this.stroke;
this.mkDiv(x,y,w,s);
this.mkDiv(x+w,y,s,h);
this.mkDiv(x,y+h,w+s,s);
this.mkDiv(x,y+s,s,h-s);
}
function jsgFont(){
this.PLAIN="font-weight:normal;";
this.BOLD="font-weight:bold;";
this.ITALIC="font-style:italic;";
this.ITALIC_BOLD=this.ITALIC+this.BOLD;
this.BOLD_ITALIC=this.ITALIC_BOLD;
}
var Font=new jsgFont();
function jsgStroke(){
this.DOTTED=-1;
}
var Stroke=new jsgStroke();
function jsGraphicsEx(cnv,wnd){
this.setColor=new Function("arg","this.color = arg.toLowerCase();");
this.setStroke=function(x){
this.stroke=x;
if(!(x+1)){
}else{
if(x-1>0){
this.drawLine=mkLin2D;
this.mkOv=mkOv2D;
this.drawRect=mkRect;
}else{
this.drawLine=mkLin;
this.mkOv=mkOv;
this.drawRect=mkRect;
}
}
};
this.setPrintable=function(arg){
this.printable=arg;
if(jg_fast){
this.mkDiv=mkDivIe;
this.htmRpc=arg?htmPrtRpc:htmRpc;
}else{
this.mkDiv=arg?mkDivPrt:mkDiv;
}
};
this.setFont=function(fam,sz,sty){
this.ftFam=fam;
this.ftSz=sz;
this.ftSty=sty||Font.PLAIN;
};
this.drawPolyline=this.drawPolyLine=function(x,y){
for(var i=x.length-1;i;){
--i;
this.drawLine(x[i],y[i],x[i+1],y[i+1]);
}
};
this.fillRect=function(x,y,w,h){
this.mkDiv(x,y,w,h);
};
this.drawPolygon=function(x,y){
this.drawPolyline(x,y);
this.drawLine(x[x.length-1],y[x.length-1],x[0],y[0]);
};
this.drawEllipse=this.drawOval=function(x,y,w,h){
this.mkOv(x,y,w,h);
};
this.drawString=function(txt,x,y,_234,_235){
this.htm+="<div style=\"position:absolute;white-space:nowrap;"+"left:"+x+"px;"+"top:"+y+"px;"+"font-family:"+this.ftFam+";"+"font-size:"+this.ftSz+";"+"color:"+_234+";"+"background-color:"+_235+";"+this.ftSty+"\">"+txt+"</div>";
};
this.clear=function(){
this.htm="";
__clearDiv(this.cnv());
};
this.mkOvQds=function(cx,cy,x,y,w,h,wod,hod){
var xl=cx-x,xr=cx+x+wod-w,yt=cy-y,yb=cy+y+hod-h;
if(xr>xl+w){
this.mkDiv(xr,yt,w,h);
this.mkDiv(xr,yb,w,h);
}else{
w=xr-xl+w;
}
this.mkDiv(xl,yt,w,h);
this.mkDiv(xl,yb,w,h);
};
this.setStroke(1);
this.setFont("verdana,geneva,helvetica,sans-serif","12px",Font.PLAIN);
this.color="#000000";
this.htm="";
this.wnd=wnd||window;
this.cnvID=cnv;
if(!jg_ok){
chkDHTM();
}
if(jg_ok){
if(cnv){
if(typeof (cnv)=="string"){
this.cont=document.all?(this.wnd.document.all[cnv]||null):document.getElementById?(this.wnd.document.getElementById(cnv)||null):null;
}else{
if(cnv==window.document){
this.cont=document.getElementsByTagName("body")[0];
}else{
this.cont=cnv;
}
}
this.paint=jg_dom?pntCnvDom:pntCnvIe;
}else{
this.paint=pntDoc;
}
}else{
this.paint=pntN;
}
this.setPrintable(false);
this.cnv=function(){
var _242=__$(this.cnvID);
var _243=_242.getElementsByTagName("DIV");
if(_243==null){
return _242;
}
return _243[0];
};
}
function CompInt(x,y){
return (x-y);
}
function map_enableAutoRefresh(){
alert("The map_enableAutoRefresh() function has been deprecated. Use the Map.set_enableAnimation property.");
}
function map_setAnimationInterval(){
alert("The map_setAnimationInterval() function has been deprecated. Use the Map.set_animationInterval property.");
}
function map_refreshAnimationLayer(){
alert("The map_refreshAnimationLayer() function has been deprecated. Use the Map.refreshAnimationLayer method.");
}

