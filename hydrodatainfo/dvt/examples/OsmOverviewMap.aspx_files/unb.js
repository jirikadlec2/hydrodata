

var r44_btime = r44_btime || new Date();
var r44_btimems = r44_btime.getTime()/1000;
var r44_smu_time = r44_smu_time || new Date().getTime();
var r44_retime, r44_retimems, r44_letime;

function initDropDownLayout() {
    var isTouchDevice = (/MSIE 10.*Touch/.test(navigator.userAgent)) || ('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch;
    var drop = jqr44('.menu-holder .drop');
    var win = jqr44(window);
    if(!drop.length) return; 

    win.bind('resize orientationchange refreshDrop', function() {
        drop.css({height: ''})
        setTimeout(function() {
            drop.css({minHeight: win.height() - drop.position().top - parseInt(drop.css('paddingBottom'))})
        }, 100)
    }).trigger('refreshDrop')
}

function initOpenClose() {

    jqr44('div.menu-holder').openClose({
        hideOnClickOutside: true,
        activeClass: 'active',
        opener: '.open',
        slider: '.drop',
        animSpeed: 400,
        effect: 'slide'
    });
    
}

function initAccordion() {
    jqr44('ul#r44universalnavbar').slideAccordion({
        activeClass: 'r44universalnavbar',
        opener: 'a.opener',
        slider: 'div.slide',
        animSpeed: 300
    });
}

;(function(jqr44) {
    function OpenClose(options) {
        this.options = jqr44.extend({
            addClassBeforeAnimation: true,
            hideOnClickOutside: false,
            activeClass:'active',
            opener:'.opener',
            slider:'.slide',
            animSpeed: 400,
            effect:'fade',
            event:'click'
        }, options);
        this.init();
    }
    OpenClose.prototype = {
        init: function() {
            if(this.options.holder) {
                this.findElements();
                this.attachEvents();
                this.makeCallback('onInit');
            }
        },
        findElements: function() {
            this.holder = jqr44(this.options.holder);
            this.opener = this.holder.find(this.options.opener);
            this.slider = this.holder.find(this.options.slider);

            if (!this.holder.hasClass(this.options.activeClass)) {
                this.slider.addClass(slideHiddenClass);
            }

            if(this.options.overlay) this.overlay = jqr44(this.options.overlay).css({opacity: 0});
        },
        attachEvents: function() {
            // add handler
            var self = this;
            this.eventHandler = function(e) {
                e.preventDefault();
                if (self.slider.hasClass(slideHiddenClass)) {
                    self.showSlide();
                } else {
                    self.hideSlide();
                }
            };
            self.opener.bind(self.options.event, this.eventHandler);

            // hover mode handler
            if(self.options.event === 'over') {
                self.opener.bind('mouseenter', function() {
                    self.holder.removeClass(self.options.activeClass);
                    self.opener.trigger(self.options.event);
                });
                self.holder.bind('mouseleave', function() {
                    self.holder.addClass(self.options.activeClass);
                    self.opener.trigger(self.options.event);
                });
            }

            // outside click handler
            if(self.options.hideOnClickOutside) {
                self.outsideClickHandler = function(e) {
                    var target = jqr44(e.target);
                    if (!target.is(self.holder) && !target.closest(self.holder).length) {
                        self.hideSlide();
                    }
                };
            }
        },
        showSlide: function() {
            var self = this;
            if (self.options.addClassBeforeAnimation) {
                self.holder.addClass(self.options.activeClass);
            }
            self.slider.removeClass(slideHiddenClass);
            jqr44(document).bind('click touchstart', self.outsideClickHandler);

            self.makeCallback('animStart', true);
            toggleEffects[self.options.effect].show({
                box: self.slider,
                speed: self.options.animSpeed,
                complete: function() {
                    if (!self.options.addClassBeforeAnimation) {
                        self.holder.addClass(self.options.activeClass);
                    }
                    self.makeCallback('animEnd', true);
                }
            });
            if(this.options.overlay) this.overlay.stop().css({display: 'block'}).fadeTo(this.options.animSpeed, 1, function() {
                jqr44(window).trigger('refreshDrop');
            });
        },
        hideSlide: function() {
            var self = this;
            if (self.options.addClassBeforeAnimation) {
                self.holder.removeClass(self.options.activeClass);
            }
            jqr44(document).unbind('click', self.outsideClickHandler);

            self.makeCallback('animStart', false);
            toggleEffects[self.options.effect].hide({
                box: self.slider,
                speed: self.options.animSpeed,
                complete: function() {
                    if (!self.options.addClassBeforeAnimation) {
                        self.holder.removeClass(self.options.activeClass);
                    }
                    self.slider.addClass(slideHiddenClass);
                    self.makeCallback('animEnd', false);
                }
            });
            if(this.options.overlay) this.overlay.stop().fadeTo(this.options.animSpeed, 0, function() {
                self.overlay.css({display: 'none'});
            });
        },
        destroy: function() {
            this.slider.removeClass(slideHiddenClass).css({display:''});
            this.opener.unbind(this.options.event, this.eventHandler);
            this.holder.removeClass(this.options.activeClass).removeData('OpenClose');
            jqr44(document).unbind('click', this.outsideClickHandler);
        },
        makeCallback: function(name) {
            if(typeof this.options[name] === 'function') {
                var args = Array.prototype.slice.call(arguments);
                args.shift();
                this.options[name].apply(this, args);
            }
        }
    };

    // add stylesheet for slide on DOMReady
    var slideHiddenClass = 'js-slide-hidden';
    jqr44(function() {
        var tabStyleSheet = jqr44('<style type="text/css">')[0];
        var tabStyleRule = '.' + slideHiddenClass;
        tabStyleRule += '{position:absolute !important;left:-9999px !important;top:-9999px !important;display:block !important}';
        if (tabStyleSheet.styleSheet) {
            tabStyleSheet.styleSheet.cssText = tabStyleRule;
        } else {
            tabStyleSheet.appendChild(document.createTextNode(tabStyleRule));
        }
        jqr44('head').append(tabStyleSheet);
    });

    // animation effects
    var toggleEffects = {
        slide: {
            show: function(o) {
                o.box.stop(true).hide().slideDown(o.speed, o.complete);
            },
            hide: function(o) {
                o.box.stop(true).css({minHeight: ''}).slideUp(o.speed, o.complete);
            }
        },
        fade: {
            show: function(o) {
                o.box.stop(true).hide().fadeIn(o.speed, o.complete);
            },
            hide: function(o) {
                o.box.stop(true).fadeOut(o.speed, o.complete);
            }
        },
        none: {
            show: function(o) {
                o.box.hide().show(0, o.complete);
            },
            hide: function(o) {
                o.box.hide(0, o.complete);
            }
        }
    };

    // jqr44 plugin interface
    jqr44.fn.openClose = function(opt) {
        return this.each(function() {
            jqr44(this).data('OpenClose', new OpenClose(jqr44.extend(opt, {holder: this})));
        });
    };
}(jqr44));

;(function(jqr44){
    jqr44.fn.slideAccordion = function(opt){
        // default options
        var options = jqr44.extend({
            addClassBeforeAnimation: false,
            activeClass:'active',
            opener:'.opener',
            slider:'.slide',
            animSpeed: 300,
            collapsible:true,
            event:'click'
        },opt);

        return this.each(function(){
            // options
            var accordion = jqr44(this);
            var items = accordion.find(':has('+options.slider+')');

            items.each(function(){
                var item = jqr44(this);
                var opener = item.find(options.opener);
                var slider = item.find(options.slider);
                opener.bind(options.event, function(e){
                    if(!slider.is(':animated')) {
                        if(item.hasClass(options.activeClass)) {
                            if(options.collapsible) {
                                slider.slideUp(options.animSpeed, function(){
                                    hideSlide(slider);
                                    item.removeClass(options.activeClass);
                                    jqr44(window).trigger('refreshDrop');
                                });
                            }
                        } else {
                            // show active
                            var levelItems = item.siblings('.'+options.activeClass);
                            var sliderElements = levelItems.find(options.slider);
                            item.addClass(options.activeClass);
                            showSlide(slider).hide().slideDown(options.animSpeed);
                        
                            // collapse others
                            sliderElements.slideUp(options.animSpeed, function(){
                                levelItems.removeClass(options.activeClass);
                                hideSlide(sliderElements);
                                jqr44(window).trigger('refreshDrop');
                            });
                        }
                    }
                    e.preventDefault();
                });
                if(item.hasClass(options.activeClass)) showSlide(slider); else hideSlide(slider);
            });
        });
    };

    // accordion slide visibility
    var showSlide = function(slide) {
        return slide.css({position:'', top: '', left: '', width: '' });
    };
    var hideSlide = function(slide) {
        return slide.show().css({position:'absolute', top: -9999, left: -9999, width: slide.width() });
    };
}(jqr44));


var r44enable = (window.location != window.parent.location) ? false : true;
var R44 = R44 || {};
R44.domain = R44.domain || '';
if (!R44.domain.length) {
    R44.domain='http://wifi.norwegian.com';
}
R44.purchased='<div class="pur"></div>';
R44.shopping='';
R44.menu_items=[];
R44.wifi_item=jqr44('<a>', {'href':R44.domain+'/wifi','class':'menu-link'}).append('WiFi');
R44.sms_item=jqr44('<a>', {'href':R44.domain+'/texting','class':'menu-link'}).append('Messaging');

R44.d_info=function(state) {
    var o=jqr44("#r44unbheader .holder .tools .holder").show();
    (state) ? o.show() : o.hide();
}
R44.d_w_info=function(state) {
    var ot=jqr44("#r44unbheader .holder .tools");
    var oh=jqr44("#r44unbheader .holder .tools .holder");
    var oh1=oh.first();
    var oh2=oh1.find(".sub-text");
    var ol=jqr44("#r44unbheader .logo ");
    if(state) {
       oh.show(); oh1.show(); oh2.show();
       ot.css('margin-left', '30px');
       ol.css('margin-left', '30px');
       oh1.css({'padding':'15px 20px 10px', 'min-width':'300px'});
    } else {
       oh.hide(); oh1.show(); oh2.hide(); 
       ot.css('margin-left', '10px');
       ol.css('margin-left', '10px');
       oh1.css({'padding':'15px 10px 10px', 'min-width':'0px'});
    }
}
R44.d_menu=function(state) {
    var o_mh=jqr44("#r44unbheader .holder .menu-holder");
    var o_mo=jqr44("#r44unbheader .holder .menu-holder .open");
    if(state) { o_mh.width(111); o_mo.width(50); o_mo.text('Menu'); } 
    else { o_mh.width(61); o_mo.width(0); o_mo.text(''); }
}
R44.set_width=function() {
    R44.width=jqr44(document).width();
    (R44.width<955) ? R44.d_menu(false) : R44.d_menu(true);
    (R44.width<900) ? R44.d_w_info(false) : R44.d_w_info(true);
    if (R44.width<480) R44.d_info(false);
    if (R44.width<250) R44.width=250;
}

R44.htmladaptor='<div id="r44unbheadermin" style="display:none; position: fixed; right:0; width:175px; bottom:0; opacity:.9" ><div id="r44ctrl" class="r44unbcontrol"><span id="r44unbshow" class="bunbshow" style="display:block"><a href="#" alt="Show Universal Navigation Bar">+</a><strong class="logomin">Norwegian</strong></span></div></div>'
+'<div id="r44unbheader" style="display:none; position: fixed; right:0; width:1000px; top:0; " ><div id="r44ctrl" class="r44unbcontrol"><span id="r44unbhide" class="bunbx"><a href="#" alt="Hide Universal Navigation Bar">x</a></span></div><strong class="logo"><a href="'+R44.domain+'">Norwegian</a></strong><div class="holder"><ul class="tools"> <li> <div class="holder">'
+'<a class="more" href="'+R44.domain+'/flight_tracker"><div class="tracker-box"> <span class="runner"><img src="'+R44.domain+'/images/unb_plane.png" width="23px" height="20px" alt="runner"></span> </div> <strong class="sub-text"><span id="c_ttgc"></span></strong></a><strong class="sub-text"><a class="more" href="'+R44.domain+'/flight_tracker">Flight Tracker</a></strong> </div> </li> <li><div id="dcity" class="holder">'
+'<a class="more" href="'+R44.domain+'/destination"><img id="c_im" alt="forecast" class="forecast" width="30px" height="30px" src="'+R44.domain+'/images/t.png"><strong class="sub-text forecast"><span id="c_tf"></span></strong></a><strong class="sub-text"><a id="fullf" class="more" href="'+R44.domain+'/destination">Full Forecast</a></strong> </div></li></ul>'
+'<div class="menu-holder"><a id="menu-opener" class="open" href="#">Menu</a><div class="drop"><ul id="r44universalnavbar"><li><a class="m_home" href="'+R44.domain+'">Home</a></li><li class="r44universalnavbar"> <a class="opener" href="#">Entertainment</a> <div class="slide"> <ul> <li><a href="'+R44.domain+'/tv">TV</a></li> <li><a href="'+R44.domain+'/movies">Movies</a></li> <li class="last"><a href="'+R44.domain+'/games">Games</a></li> </ul> </div> </li> <li> <a class="opener" href="#">Connect</a> <div class="slide"> <ul id="connect"> </ul> </div> </li> <li> <a class="opener" href="'+R44.domain+'/destination">Destination</a> <div class="slide"> <ul> <li><a href="'+R44.domain+'/flight_tracker">Flight Tracker</a></li><li class="last"><a href="'+R44.domain+'/destination">Destination Info</a></li></ul></div></li><li><a class="opener" href="#">Visit Norwegian.com</a> <div class="slide"> <ul><li><a class="m_flight" href="http://www.norwegian.com/flight/" target="_blank" >Flights<span class="arrow"></span></a></li><li><a class="m_hotel" href="http://www.norwegian.com/hotels" target="_blank" >Hotels<span class="arrow"></span></a></li><li class="last"><a class="m_car" href="http://www.norwegian.com/car-rental" target="_blank" >Cars<span class="arrow"></span></a></li></ul></div></li></ul>'
+'<div id="menu_bottom" class="holder"> <p><a href="http://www.norwegian.com/" target="_blank" >Norwegian.com</a> | <a href="http://www.norwegian.com/norwegianreward" target="_blank" >Norwegian Reward</a></p> </div></div></div></div></div>';

R44.createadaptor=function() {
    var f=document.createDocumentFragment();
    var e=document.createElement('div');
    e.innerHTML = R44.htmladaptor;
    while (e.firstChild) {
        f.appendChild(e.firstChild);
    }
    return f;
}
     
R44.set_runner=function(val) {  
    min_pixel_pos=0;
    max_pixel_pos=180;
    var runner = jqr44('.tracker-box .runner');
    if(!runner.length) return;
    val = ( val < min_pixel_pos ) ? min_pixel_pos : val;
    val = ( val > max_pixel_pos ) ? max_pixel_pos : val;
    runner.css({left: val});
}                   

R44.current_callback=function(response) {
    if (response.pcent_flt_complete) {
        dist_ratio = (response.pcent_flt_complete*180/70|0);
        R44.set_runner(dist_ratio);
    }
    if (response.ttgc) { jqr44('#c_ttgc').text(response.ttgc); }
    if (response.currentC) { jqr44('#c_tf').text(response.currentC); }
    if (response.dest_code) { jqr44('#c_dest').text(response.dest_code); }
    if (response.image_white) {
        jqr44('#c_im').attr('src',R44.domain+'/images/'+response.image_white);
    }
    else {
        jqr44('#fullf').hide();
    }
    if (response.dest_city) { jqr44('#dcity').prop('title', response.dest_city); }  // TLG
}

R44.m_callback=function(response) {
    jqr44("#r44universalnavbar ul#connect li").remove()
    var has_wifi = false;
    var has_sms  = false;
    jqr44.each(response.packages, function(p_i, p_v) {
        if (p_v=='WIFI') { has_wifi=true; }
        if (p_v=='SMS') { has_sms=true; }
    });
    if (has_wifi) { R44.wifi_item.append(R44.purchased) }
    R44.menu_items.push(R44.wifi_item);
    if (response.features.sms) { 
        if (has_wifi||has_sms) { R44.sms_item.append(R44.purchased) }
        R44.menu_items.push(R44.sms_item);
    }
    jqr44("#r44universalnavbar ul#connect li").remove();
    jqr44.each(R44.menu_items, function(k,v) {
        var last=(k==R44.menu_items.length-1)? "last" : "";
        jqr44("#r44universalnavbar ul#connect").append(jqr44('<li>', {'class':last}).append(v));
    });
    if (response.ad.imagePath && response.ad.imagePath.length) {
        var link = response.ad.link;
        if (link.charAt(0) == '/') { link = R44.domain + response.ad.link;}
        var ad_img = jqr44('<img />').attr({width:'300', height:'250', alt:response.ad.caption, src:R44.domain+response.ad.imagePath});
        jqr44("#r44unbheader div.drop #menu_bottom").append(jqr44('<a>', {id: 'ad_unb', 'class':'menu-link', target:response.ad.target, href:link}).append(ad_img));
        jqr44("#ad_unb").data("campaign", response.ad.campaign).data("image", response.ad.imagePath).data("adid", response.ad.id);
        jqr44("#menu-opener").click(function(e) {
            R44.ad_log('ad_unb', 'view');
        });

        jqr44("#ad_unb").click(function(e) {
            R44.ad_log('ad_unb', 'click');
        });
    }
    if (response.unb_state) { R44.show_unb(); } else { R44.hide_unb(); }
}

R44.m_call=function() {
    var c_url = R44.domain+'/mp.json';
        jqr44.ajax({
            url: c_url,
            dataType: 'jsonp',
            jsonpCallback: 'R44.m_callback',
            error: function(results){
            }
        });
}
R44.at_call=function(data) {
    var c_url = R44.domain+'/ap.json';
        jqr44.ajax({
            url: c_url,
            data: data,
            dataType: 'jsonp',
            error: function(results){
            }
        });
}
R44.unb_call=function(data) {
    var c_url = R44.domain+'/unb.json';
        jqr44.ajax({
            url: c_url,
            data: data,
            dataType: 'jsonp',
            error: function(results){
            }
        });
}

R44.current_call=function() {
    var c_url = R44.domain+'/currentp.json';
        jqr44.ajax({
            url: c_url,
            dataType: 'jsonp',
            jsonpCallback: 'R44.current_callback',
            error: function(results){
            }
        });
}

R44.ad_log=function(adType, adAction) {
    try {
        obj = jqr44("#ad_unb");
        var c_url = R44.domain + '/logAdData';

        jqr44.ajax({
            url: c_url,
            data: { 'adCampaign': obj.data('campaign'), 'adImage': obj.data('image'), 'adId': obj.data('adid'), 'adType': adType, 'adAction': adAction, 'page': 'unb' },
            dataType: 'jsonp',
            timeout: 2000,
            success: function(){
            },
            error: function(results){
            }
        });
    }
    catch(e) {
    }
}

R44.show_unb=function() {
    jqr44("#r44unbheadermin").fadeOut("slow");
    jqr44("#r44unbheader").fadeIn("slow");
}
R44.hide_unb=function() {
    jqr44("#r44unbheader").fadeOut("slow");
    jqr44("#r44unbheadermin").fadeIn("slow");
}
R44.set_unb=function() {
    var max_index=2147483647;
    var o_layers=jqr44('div').filter(function(){ return (jqr44(this).css('z-index') == max_index && jqr44(this).attr('id')!="r44unbheader" && jqr44(this).attr('id')!= "r44unbheadermin")})
    o_layers.css({"z-index":max_index-10})
}

jqr44(window).resize(function() {
    R44.set_width();
    jqr44("#r44unbheader").css({width:R44.width});
});

jqr44(document).ready(function() {
    r44_retime = new Date();
    r44_retimems = r44_retime.getTime()/1000;

    if (!r44enable) return;
    if (r44enable) { document.body.appendChild(R44.createadaptor()); }

    R44.m_call();
    R44.set_unb();
    jqr44('#r44unbhide').click(function(e) {
            R44.hide_unb();
            R44.unb_call({'s':false});
            e.preventDefault();
    });
    jqr44('#r44unbshow').click(function(e) {
            R44.show_unb();
            R44.unb_call({'s':true});
            e.preventDefault();
    });
    jqr44(function(){
        initOpenClose();
        initAccordion();
        initDropDownLayout();
    });
    R44.set_width();
    jqr44("#r44unbheader").css({width:R44.width});

    R44.current_call();setInterval(function() { R44.current_call(); }, 120000);
    
});
jqr44(window).load(function() {
    if (!r44enable) return;
    r44_letime = new Date();
    r44_letimems = r44_letime.getTime()/1000;
    var r44_ltime = r44_letimems-r44_btimems;
    var r44_rtime = r44_retimems-r44_btimems;
    var r44data = {"url":window.location.href, "smu_start_time": r44_smu_time, "start_time":r44_btimems, "js_load_time":r44_ltime, "js_ready_time":r44_rtime};
    R44.at_call(r44data);
});



