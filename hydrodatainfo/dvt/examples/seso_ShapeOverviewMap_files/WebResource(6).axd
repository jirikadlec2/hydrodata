if(typeof (Type)!=="undefined"){
Type.registerNamespace("AspMap");
}else{
eval("var AspMap = {};");
}
AspMap.find=function(_1){
try{
return eval("_"+_1+"obj");
}
catch(ex){
}
return null;
};
AspMap.Map=function(){
this._map=null;
};
AspMap.Map.prototype={get_zoomLevel:function(){
return this._map.GetZoom();
},set_zoomLevel:function(_2){
this._map.SetZoom(_2);
},get_zoomLevelCount:function(){
return this._map.GetZoomLevelCount();
},get_mapScale:function(){
return this._map.GetMapScale();
},set_mapScale:function(_3){
this._map.SetMapScale(_3);
},get_cursor:function(){
return this._map.GetCursor();
},set_cursor:function(_4){
this._map.SetCursor(_4);
},get_mapTool:function(){
return this._map.GetTool();
},set_mapTool:function(_5){
this._map.SetTool(_5);
},get_mapToolArgument:function(){
return this._map.GetArgument();
},set_mapToolArgument:function(_6){
this._map.SetArgument(_6);
},get_enableAnimation:function(){
return this._map.GetEnableAnimation();
},set_enableAnimation:function(_7){
this._map.SetEnableAnimation(_7);
},get_visible:function(){
return this._map.IsVisible();
},set_visible:function(_8){
this._map.SetVisible(_8);
},get_animationInterval:function(){
return this._map.GetAnimationInterval();
},set_animationInterval:function(_9){
this._map.SetAnimationInterval(_9);
},get_width:function(){
return this._map.MapWidth();
},get_height:function(){
return this._map.MapHeight();
},get_backgroundLayerMapObject:function(){
if(!map._map.backLayer){
return null;
}
return this._map.backLayer.mapObj;
},pan:function(_a,_b){
this._map.Pan(_a,_b);
},centerAt:function(_c){
this._map.CenterAt(_c.x,_c.y);
},centerAndZoom:function(_d,_e){
this._map.CenterAndZoom(_d.x,_d.y,_e);
},centerAndScale:function(_f,_10){
this._map.CenterAndScale(_f.x,_f.y,_10);
},zoomIn:function(){
this._map.ZoomIn();
},zoomOut:function(){
this._map.ZoomOut();
},zoomFull:function(){
this._map.ZoomFull();
},refresh:function(){
this._map.Refresh();
},refreshAnimationLayer:function(){
this._map.RefreshAnimationLayer();
},resizeTo:function(_11,_12){
this._map.ResizeTo(_11,_12);
},resize:function(){
this._map.Resize();
},print:function(){
this._map.Print();
},add_mouseMove:function(_13){
this._map.Add_MouseMove(_13);
},remove_mouseMove:function(_14){
this._map.Remove_MouseMove(_14);
},add_panToolClick:function(_15){
this._map.Add_PanToolClick(_15);
},remove_panToolClick:function(_16){
this._map.Remove_PanToolClick(_16);
},add_pointTool:function(_17){
this._map.Add_PointTool(_17);
},remove_pointTool:function(_18){
this._map.Remove_PointTool(_18);
},add_markerClick:function(_19){
this._map.Add_MarkerClick(_19);
},remove_markerClick:function(_1a){
this._map.Remove_MarkerClick(_1a);
},add_infoTool:function(_1b){
this._map.Add_InfoTool(_1b);
},remove_infoTool:function(_1c){
this._map.Remove_InfoTool(_1c);
}};
AspMap.Point=function(x,y){
this.x=0;
this.y=0;
if(typeof (x)!=="undefined"){
this.x=x;
}
if(typeof (y)!=="undefined"){
this.y=y;
}
};
AspMap.MouseEventArgs=function(){
this.x=0;
this.y=0;
this.longitude=0;
this.latitude=0;
this.isInside=false;
this.mapPoint=null;
};
AspMap.MarkerClickEventArgs=function(){
this.argument="";
this.content="";
};
AspMap.MapTool=function(){
};
AspMap.MapTool.prototype={ZoomIn:1,ZoomOut:2,Center:3,Pan:4,Info:5,Distance:6,InfoWindow:7,Point:8,Line:9,Polyline:10,Rectangle:11,Circle:12,Polygon:13};
if(typeof (Type)!=="undefined"){
AspMap.Map.registerClass("AspMap.Map");
AspMap.Point.registerClass("AspMap.Point");
AspMap.MouseEventArgs.registerClass("AspMap.MouseEventArgs");
AspMap.MarkerClickEventArgs.registerClass("AspMap.MarkerClickEventArgs");
AspMap.MapTool.registerEnum("AspMap.MapTool");
}else{
for(var i in AspMap.MapTool.prototype){
AspMap.MapTool[i]=AspMap.MapTool.prototype[i];
}
}

