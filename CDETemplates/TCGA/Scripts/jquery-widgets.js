/* Yahoo style news slider, used on multimedia library on News and Events page */

/****************************************************************************
Accessible News Slider

courtesy:
https://github.com/rip747/Yahoo-style-news-slider-for-jQuery

Author:
Brian Reindel, modified and adapted by Andrea Ferracani

Author URL:
http://blog.reindel.com, http://www.micc.unifi.it/ferracani

License:
Unrestricted. This script is free for both personal and commercial use.
*****************************************************************************/
(function( $ ){
	$.fn.accessNews = function(settings){
	
		var defaults = {
			// title for the display
			title: "TODAY NEWS:",
			// subtitle for the display
			subtitle: "November 27 2010",
			// number of slides to advance when pagnating
			slideBy: 3,
			// the speed for the pagination
			speed: "normal",
			// slideshow interval
			slideShowInterval: 15000,
			// delay before slide show begins
			slideShowDelay: 15000,
			// theme
			theme: "business_as_usual"
		};
		
		return this.each(function(){
			
			settings = jQuery.extend(defaults, settings);
			var _this = jQuery(this);
			var stories = _this.children();
			var intervalId = null; 
			
			var container = {
			
				_wrapper: "<div class=\"accessible_news_slider " + settings.theme + "\"></div>",
				_container: "<div class=\"container\"></div>",
				// We are not using the Title and Subtitle features
				/* _headline: jQuery("<div class='headline'></div>").html(["<p><strong>", settings.title, "</strong> ", settings.subtitle, "</p>"].join("")), */
				_content: jQuery("<div class='content'></div>"),
				_first: jQuery(stories[0]),
				
				init: function(){
					// wrap the ul with our div class and assigned theme
					_this.wrap(this._wrapper);
					// our container where we show the image and news item
					_this.before(this._container);
					// set the width of the container
					_this.css("width", (stories.length * this._first.width()));
					this.append(this._headline);
					this.append(this._content);
					this.set(this._first);
				},
				
				append: function(content){
					this.get().append(content);
				},
				
				// returns the main container
				get: function(){
					return _this.prev("div.container");
				},
				
				set: function(story){
					var container = this.get();
					var _content = jQuery("div.content", container);
					var img = jQuery('<img></img>');
					var para = jQuery('<div></div>');
					var title = jQuery('p.title a', story);
					img.attr('src', jQuery('img', story).attr('src'));
					img.attr('alt', jQuery('img', story).attr('alt'));
					title = title.attr('title') || title.text();
					para.html("<h1>" + title + "</h1>" + "<p class='paraText'>" + jQuery('p.description', story).html() + "</p>");
					stories.removeClass('selected');
					story.addClass('selected');
					_content.empty();
					_content.append(img);
					_content.append(para);
				}
				
			};
			
			var pagination = {
			
				loaded: false,
				_animating: false,
				_totalPages: 0,
				_currentPage: 1,
				_storyWidth: 0,
				_slideByWidth: 0,
				_totalWidth: 0,

				init: function(){
					if (stories.length > settings.slideBy) {
						this._totalPages = Math.ceil(stories.length / settings.slideBy);
						this._storyWidth = jQuery(stories[0]).width();
						this._slideByWidth = this._storyWidth * settings.slideBy;
						this._totalWidth = this._storyWidth * stories.length;
						this.draw();
						this.loaded = true;
					}
				},
				
				draw: function(){
				
					var _viewAll = jQuery("<div class=\"view_all\"></div>").html(["<div class=\"count\"><span class=\"startAt\">1</span> - <span class=\"endAt\">", settings.slideBy, "</span> of ", stories.length, "</span></div><div class=\"controls\"><span class=\"back\"><a href=\"#\" title=\"Back\">&lt;&lt; Back</a></span><span class=\"next\"><a href=\"#\" title=\"Next\">Next &gt;&gt;</a></span></div>"].join(""));
					_this.after(_viewAll);
					
					var _next = jQuery(".next > a", _viewAll);
					var _back = jQuery(".back > a", _viewAll);
					
					_next.click(function(){
						
						var page = pagination._currentPage + 1;
						pagination.to(page);
						return false;
						
					});
					
					_back.click(function(){
						
						var page = pagination._currentPage - 1;
						pagination.to(page);
						return false;
						
					});

				},
				
				to: function(page){

					if(this._animating){
						return;
					}
					
					var viewAll = _this.next(".view_all");
					var startAt = jQuery(".startAt", viewAll);
					var endAt = jQuery(".endAt", viewAll);

					this._animating = true;
					
					if(page >= this._totalPages)
					{
						page = this._totalPages;
					}
					
					if (page <= 1)
					{
						page = 1;
					}

					var _startAt = (page * settings.slideBy) - settings.slideBy;
					var _left = parseInt(_this.css("left"));
					var _offset = (page * this._slideByWidth) - this._slideByWidth;
					startAt.html(_startAt + 1);
					endAt.html(page * settings.slideBy);
					
					_left = (_offset * -1);
						
					_this.animate({
						left: _left
					}, settings.speed);
					
					// when paginating set the active story to the first
					// story on the page
					
					container.set(jQuery(stories[_startAt]));

					this._currentPage = page;
					this._animating = false;
						
				}

			};
			
			var slideshow = {
				
				init: function(){
					this.attach();
					setTimeout(function(){
						intervalId = "";
						slideshow.on();
					}, settings.slideShowDelay);
				},
				
				on: function(){
					if (!intervalId) {
						intervalId = setInterval(function(){
							slideshow.slide();
						}, settings.slideShowInterval);
					}
				},
				
				off: function(){
					if (intervalId) {
						clearInterval(intervalId);
						intervalId = null;
					}
				},
				
				slide: function(){
					
					var current = jQuery("li.selected", _this);
					var next = current.next("li");
					var page = 0;
					var storyIndex = 0;
					var storyMod = 0;
					
					if (!next.length)
					{
						next = jQuery(stories[0]);
						page = 1;
					}

					container.set(next);
					
					if (pagination.loaded) {
						storyIndex = stories.index(next);
						storyMod = (storyIndex) % settings.slideBy;
						
						if (storyMod === 0) {
							page = (Math.ceil(storyIndex / settings.slideBy)) + 1;
						}
						
						if (page > 0) {
							pagination.to(page);
						}
					}
				},
				
				attach: function(){
					
					var that = jQuery(_this).parent("div.accessible_news_slider");
					that.hover(function(){
						// pause the slideshow on hover
						slideshow.off();
					}, function (){
						// resume slideshow on mouseout
						slideshow.on();
					});
					
				}
				
			};
			
			//setup the container
			container.init();
			// pagination setup
			pagination.init();
			// slideshow setup
			 slideshow.init(); 
			// append hover every to each element to update container content
			stories.hover(function(){
				// set container contect to hovered li
				container.set(jQuery(this));
			}, function(){
				// do nothing
			});

		});
	};
})( jQuery );
/* END Yahoo style news slider, used on multimedia library on News and Events page */

/* Begin How it Works widget */
/*
 * 	easyAccordion 0.1 - jQuery plugin
 *	written by Andrea Cima Serniotti	
 *	http://www.madeincima.eu
 *
 *	Copyright (c) 2010 Andrea Cima Serniotti (http://www.madeincima.eu)
 *	Dual licensed under the MIT (MIT-LICENSE.txt) and GPL (GPL-LICENSE.txt) licenses.
 *	Built for jQuery library http://jquery.com
 */
 
(function(jQuery) {
	jQuery.fn.easyAccordion = function(options) {
	
	var defaults = {			
		slideNum: true,
		autoStart: false,
		slideInterval: 7000
	};
			
	this.each(function() {
		
		var settings = jQuery.extend(defaults, options);		
		jQuery(this).find('dl').addClass('easy-accordion');
		
		
		// -------- Set the variables ------------------------------------------------------------------------------
		
		jQuery.fn.setVariables = function() {
			dlWidth = jQuery(this).width();
			dlHeight = jQuery(this).height();
			dtWidth = jQuery(this).find('dt').outerHeight();
			if (jQuery.browser.msie){ dtWidth = $(this).find('dt').outerWidth();}
			dtHeight = dlHeight - (jQuery(this).find('dt').outerWidth()-jQuery(this).find('dt').width());
			slideTotal = jQuery(this).find('dt').size();
			ddWidth = dlWidth - (dtWidth*slideTotal) - (jQuery(this).find('dd').outerWidth(true)-jQuery(this).find('dd').width());
			ddHeight = dlHeight - (jQuery(this).find('dd').outerHeight(true)-jQuery(this).find('dd').height());
		};
		jQuery(this).setVariables();
	
		
		// -------- Fix some weird cross-browser issues due to the CSS rotation -------------------------------------

		if (jQuery.browser.safari){ var dtTop = (dlHeight-dtWidth)/2; var dtOffset = -dtTop;  /* Safari and Chrome */ }
		if (jQuery.browser.mozilla){ var dtTop = dlHeight - 20; var dtOffset = - 20; /* FF */ }
		if (jQuery.browser.msie){ var dtTop = 0; var dtOffset = 0; /* IE */ }
		
		
		// -------- Getting things ready ------------------------------------------------------------------------------
		
		var f = 1;
		jQuery(this).find('dt').each(function(){
			jQuery(this).css({'width':dtHeight,'top':dtTop,'margin-left':dtOffset});	
			if(settings.slideNum == true){
				jQuery('<span class="slide-number">'+0+f+'</span>').appendTo(this);
				if(jQuery.browser.msie){	
					var slideNumLeft = parseInt(jQuery(this).find('.slide-number').css('left')) - 14;
					jQuery(this).find('.slide-number').css({'left': slideNumLeft})
					if(jQuery.browser.version == 6.0 || jQuery.browser.version == 7.0){
						jQuery(this).find('.slide-number').css({'bottom':'auto'});
					}
					if(jQuery.browser.version == 8.0){
					var slideNumTop = jQuery(this).find('.slide-number').css('bottom');
					var slideNumTopVal = parseInt(slideNumTop) + parseInt(jQuery(this).css('padding-top'))  - 12; 
					jQuery(this).find('.slide-number').css({'bottom': slideNumTopVal}); 
					}
				} else {
					var slideNumTop = jQuery(this).find('.slide-number').css('bottom');
					var slideNumTopVal = parseInt(slideNumTop) + parseInt(jQuery(this).css('padding-top')); 
					jQuery(this).find('.slide-number').css({'bottom': slideNumTopVal}); 
				}
			}
			f = f + 1;
		});
		
		if(jQuery(this).find('.active').size()) { 
			jQuery(this).find('.active').next('dd').addClass('active');
		} else {
			jQuery(this).find('dt:first').addClass('active').next('dd').addClass('active');
		}
		
		jQuery(this).find('dt:first').css({'left':'0'}).next().css({'left':dtWidth});
		jQuery(this).find('dd').css({'width':ddWidth,'height':ddHeight});	

		
		// -------- Functions ------------------------------------------------------------------------------
		
		jQuery.fn.findActiveSlide = function() {
				var i = 1;
				this.find('dt').each(function(){
				if(jQuery(this).hasClass('active')){
					activeID = i; // Active slide
				} else if (jQuery(this).hasClass('no-more-active')){
					noMoreActiveID = i; // No more active slide
				}
				i = i + 1;
			});
		};
			
		jQuery.fn.calculateSlidePos = function() {
			var u = 2;
			jQuery(this).find('dt').not(':first').each(function(){	
				var activeDtPos = dtWidth*activeID;
				if(u <= activeID){
					var leftDtPos = dtWidth*(u-1);
					jQuery(this).animate({'left': leftDtPos});
					if(u < activeID){ // If the item sits to the left of the active element
						jQuery(this).next().css({'left':leftDtPos+dtWidth});	
					} else{ // If the item is the active one
						jQuery(this).next().animate({'left':activeDtPos});
					}
				} else {
					var rightDtPos = dlWidth-(dtWidth*(slideTotal-u+1));
					jQuery(this).animate({'left': rightDtPos});
					var rightDdPos = rightDtPos+dtWidth;
					jQuery(this).next().animate({'left':rightDdPos});	
				}
				u = u+ 1;
			});
			setTimeout( function() {
				jQuery('.easy-accordion').find('dd').not('.active').each(function(){ 
					jQuery(this).css({'display':'none'});
				});
			}, 400);
			
		};
	
		jQuery.fn.activateSlide = function() {
			this.parent('dl').setVariables();	
			this.parent('dl').find('dd').css({'display':'block'});
			this.parent('dl').find('dd.plus').removeClass('plus');
			this.parent('dl').find('.no-more-active').removeClass('no-more-active');
			this.parent('dl').find('.active').removeClass('active').addClass('no-more-active');
			this.addClass('active').next().addClass('active');	
			this.parent('dl').findActiveSlide();
			if(activeID < noMoreActiveID){
				this.parent('dl').find('dd.no-more-active').addClass('plus');
			}
			this.parent('dl').calculateSlidePos();	
		};
	
		jQuery.fn.rotateSlides = function(slideInterval, timerInstance) {
			var accordianInstance = jQuery(this);
			timerInstance.value = setTimeout(function(){accordianInstance.rotateSlides(slideInterval, timerInstance);}, slideInterval);
			jQuery(this).findActiveSlide();
			var totalSlides = jQuery(this).find('dt').size();
			var activeSlide = activeID;
			var newSlide = activeSlide + 1;
			if (newSlide > totalSlides) newSlide = 1;
			jQuery(this).find('dt:eq(' + (newSlide-1) + ')').activateSlide(); // activate the new slide
		}


		// -------- Let's do it! ------------------------------------------------------------------------------
		
		function trackerObject() {this.value = null}
		var timerInstance = new trackerObject();
		
		jQuery(this).findActiveSlide();
		jQuery(this).calculateSlidePos();
		
		if (settings.autoStart == true){
			var accordianInstance = jQuery(this);
			var interval = parseInt(settings.slideInterval);
			timerInstance.value = setTimeout(function(){
				accordianInstance.rotateSlides(interval, timerInstance);
				}, interval);
		} 

		jQuery(this).find('dt').not('active').click(function(){		
			jQuery(this).activateSlide();
			clearTimeout(timerInstance.value);
		});	
				
		if (!(jQuery.browser.msie && jQuery.browser.version == 6.0)){ 
			jQuery('dt').hover(function(){
				jQuery(this).addClass('hover');
			}, function(){
				jQuery(this).removeClass('hover');
			});
		}
	});
	};
})(jQuery);
/* END How it Works widget */