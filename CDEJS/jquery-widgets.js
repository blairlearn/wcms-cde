/* BEGIN Yahoo style news slider, used on multimedia library on News and Events page */

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

(function ($) {
    $.fn.accessNews = function (settings) {

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

        return this.each(function () {

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

                init: function () {
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

                append: function (content) {
                    this.get().append(content);
                },

                // returns the main container
                get: function () {
                    return _this.prev("div.container");
                },

                set: function (story) {
                    var container = this.get();
                    var _content = jQuery("div.content", container);
                    var img = jQuery('<img></img>');
                    var para = jQuery('<div></div>');
                    var title = jQuery('p.title a', story);
                    img.attr('src', jQuery('img', story).attr('src'));
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

                init: function () {
                    if (stories.length > settings.slideBy) {
                        this._totalPages = Math.ceil(stories.length / settings.slideBy);
                        this._storyWidth = jQuery(stories[0]).width();
                        this._slideByWidth = this._storyWidth * settings.slideBy;
                        this._totalWidth = this._storyWidth * stories.length;
                        this.draw();
                        this.loaded = true;
                    }
                },

                draw: function () {

                    var _viewAll = jQuery("<div class=\"view_all\"></div>").html(["<div class=\"count\"><span class=\"startAt\">1</span> - <span class=\"endAt\">", settings.slideBy, "</span> of ", stories.length, "</span></div><div class=\"controls\"><span class=\"back\"><a href=\"#\" title=\"Back\">&lt;&lt; Back</a></span><span class=\"next\"><a href=\"#\" title=\"Next\">Next &gt;&gt;</a></span></div>"].join(""));
                    _this.after(_viewAll);

                    var _next = jQuery(".next > a", _viewAll);
                    var _back = jQuery(".back > a", _viewAll);

                    _next.click(function () {

                        var page = pagination._currentPage + 1;
                        pagination.to(page);
                        return false;

                    });

                    _back.click(function () {

                        var page = pagination._currentPage - 1;
                        pagination.to(page);
                        return false;

                    });

                },

                to: function (page) {

                    if (this._animating) {
                        return;
                    }

                    var viewAll = _this.next(".view_all");
                    var startAt = jQuery(".startAt", viewAll);
                    var endAt = jQuery(".endAt", viewAll);

                    this._animating = true;

                    if (page >= this._totalPages) {
                        page = this._totalPages;
                    }

                    if (page <= 1) {
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

                init: function () {
                    this.attach();
                    setTimeout(function () {
                        intervalId = "";
                        slideshow.on();
                    }, settings.slideShowDelay);
                },

                on: function () {
                    if (!intervalId) {
                        intervalId = setInterval(function () {
                            slideshow.slide();
                        }, settings.slideShowInterval);
                    }
                },

                off: function () {
                    if (intervalId) {
                        clearInterval(intervalId);
                        intervalId = null;
                    }
                },

                slide: function () {

                    var current = jQuery("li.selected", _this);
                    var next = current.next("li");
                    var page = 0;
                    var storyIndex = 0;
                    var storyMod = 0;

                    if (!next.length) {
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

                attach: function () {

                    var that = jQuery(_this).parent("div.accessible_news_slider");
                    that.hover(function () {
                        // pause the slideshow on hover
                        slideshow.off();
                    }, function () {
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
            stories.hover(function () {
                // set container contect to hovered li
                container.set(jQuery(this));
            }, function () {
                // do nothing
            });

        });
    };
})(jQuery); /* END Yahoo style news slider, used on multimedia library on News and Events page */


/* BEGIN Home Page Tile Slider */
/*
* Slides, A Slideshow Plugin for jQuery
* Intructions: http://slidesjs.com
* By: Nathan Searles, http://nathansearles.com
* Version: 1.1.4
* Updated: February 25th, 2011
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
(function ($) {
    $.fn.slides = function (option) {
        option = $.extend({}, $.fn.slides.option, option);
        return this.each(function () {
            $('.' + option.container, $(this)).children().wrapAll('<div class="slides_control"/>');
            var elem = $(this),
                control = $('.slides_control', elem),
                total = control.children().size(),
                width = control.children().outerWidth(),
                height = control.children().outerHeight(),
                start = option.start - 1,
                effect = option.effect.indexOf(',') < 0 ? option.effect : option.effect.replace(' ', '').split(',')[0],
                paginationEffect = option.effect.indexOf(',') < 0 ? effect : option.effect.replace(' ', '').split(',')[1],
                next = 0,
                prev = 0,
                number = 0,
                current = 0,
                loaded, active, clicked, position, direction, imageParent, pauseTimeout, playInterval;

            function animate(direction, effect, clicked) {
                if (!active && loaded) {
                    active = true;
                    option.animationStart(current + 1);
                    switch (direction) {
                    case 'next':
                        prev = current;
                        next = current + 1;
                        next = total === next ? 0 : next;
                        position = width * 2;
                        direction = -width * 2;
                        current = next;
                        break;
                    case 'prev':
                        prev = current;
                        next = current - 1;
                        next = next === -1 ? total - 1 : next;
                        position = 0;
                        direction = 0;
                        current = next;
                        break;
                    case 'pagination':
                        next = parseInt(clicked, 10);
                        prev = $('.' + option.paginationClass + ' li.current a', elem).attr('href').match('[^#/]+$');
                        if (next > prev) {
                            position = width * 2;
                            direction = -width * 2;
                        } else {
                            position = 0;
                            direction = 0;
                        }
                        current = next;
                        break;
                    }
                    if (effect === 'fade') {
                        if (option.crossfade) {
                            control.children(':eq(' + next + ')', elem).css({
                                zIndex: 10
                            }).fadeIn(option.fadeSpeed, option.fadeEasing, function () {
                                if (option.autoHeight) {
                                    control.animate({
                                        height: control.children(':eq(' + next + ')', elem).outerHeight()
                                    }, option.autoHeightSpeed, function () {
                                        control.children(':eq(' + prev + ')', elem).css({
                                            display: 'none',
                                            zIndex: 0
                                        });
                                        control.children(':eq(' + next + ')', elem).css({
                                            zIndex: 0
                                        });
                                        option.animationComplete(next + 1);
                                        active = false;
                                    });
                                } else {
                                    control.children(':eq(' + prev + ')', elem).css({
                                        display: 'none',
                                        zIndex: 0
                                    });
                                    control.children(':eq(' + next + ')', elem).css({
                                        zIndex: 0
                                    });
                                    option.animationComplete(next + 1);
                                    active = false;
                                }
                            });
                        } else {
                            control.children(':eq(' + prev + ')', elem).fadeOut(option.fadeSpeed, option.fadeEasing, function () {
                                if (option.autoHeight) {
                                    control.animate({
                                        height: control.children(':eq(' + next + ')', elem).outerHeight()
                                    }, option.autoHeightSpeed, function () {
                                        control.children(':eq(' + next + ')', elem).fadeIn(option.fadeSpeed, option.fadeEasing);
                                    });
                                } else {
                                    control.children(':eq(' + next + ')', elem).fadeIn(option.fadeSpeed, option.fadeEasing, function () {
                                        if ($.browser.msie) {
                                            $(this).get(0).style.removeAttribute('filter');
                                        }
                                    });
                                }
                                option.animationComplete(next + 1);
                                active = false;
                            });
                        }
                    } else {
                        control.children(':eq(' + next + ')').css({
                            left: position,
                            display: 'block'
                        });
                        if (option.autoHeight) {
                            control.animate({
                                left: direction,
                                height: control.children(':eq(' + next + ')').outerHeight()
                            }, option.slideSpeed, option.slideEasing, function () {
                                control.css({
                                    left: -width
                                });
                                control.children(':eq(' + next + ')').css({
                                    left: width,
                                    zIndex: 5
                                });
                                control.children(':eq(' + prev + ')').css({
                                    left: width,
                                    display: 'none',
                                    zIndex: 0
                                });
                                option.animationComplete(next + 1);
                                active = false;
                            });
                        } else {
                            control.animate({
                                left: direction
                            }, option.slideSpeed, option.slideEasing, function () {
                                control.css({
                                    left: -width
                                });
                                control.children(':eq(' + next + ')').css({
                                    left: width,
                                    zIndex: 5
                                });
                                control.children(':eq(' + prev + ')').css({
                                    left: width,
                                    display: 'none',
                                    zIndex: 0
                                });
                                option.animationComplete(next + 1);
                                active = false;
                            });
                        }
                    }
                    if (option.pagination) {
                        $('.' + option.paginationClass + ' li.current', elem).removeClass('current');
                        $('.' + option.paginationClass + ' li:eq(' + next + ')', elem).addClass('current');
                    }
                }
            }

            function stop() {
                clearInterval(elem.data('interval'));
            }

            function pause() {
                if (option.pause) {
                    clearTimeout(elem.data('pause'));
                    clearInterval(elem.data('interval'));
                    pauseTimeout = setTimeout(function () {
                        clearTimeout(elem.data('pause'));
                        playInterval = setInterval(function () {
                            animate("next", effect);
                        }, option.play);
                        elem.data('interval', playInterval);
                    }, option.pause);
                    elem.data('pause', pauseTimeout);
                } else {
                    stop();
                }
            }
            if (total < 2) {
                return;
            }
            if (start < 0) {
                start = 0;
            }
            if (start > total) {
                start = total - 1;
            }
            if (option.start) {
                current = start;
            }
            if (option.randomize) {
                control.randomize();
            }
            $('.' + option.container, elem).css({
                overflow: 'hidden',
                position: 'relative'
            });
            control.children().css({
                position: 'absolute',
                top: 0,
                left: control.children().outerWidth(),
                zIndex: 0,
                display: 'none'
            });
            control.css({
                position: 'relative',
                width: (width * 3),
                height: height,
                left: -width
            });
            $('.' + option.container, elem).css({
                display: 'block'
            });
            if (option.autoHeight) {
                control.children().css({
                    height: 'auto'
                });
                control.animate({
                    height: control.children(':eq(' + start + ')').outerHeight()
                }, option.autoHeightSpeed);
            }
            if (option.preload && control.find('img').length) {
                $('.' + option.container, elem).css({
                    background: 'url(' + option.preloadImage + ') no-repeat 50% 50%'
                });
                var img = control.find('img:eq(' + start + ')').attr('src') + '?' + (new Date()).getTime();
                if ($('img', elem).parent().attr('class') != 'slides_control') {
                    imageParent = control.children(':eq(0)')[0].tagName.toLowerCase();
                } else {
                    imageParent = control.find('img:eq(' + start + ')');
                }
                control.find('img:eq(' + start + ')').attr('src', img).load(function () {
                    control.find(imageParent + ':eq(' + start + ')').fadeIn(option.fadeSpeed, option.fadeEasing, function () {
                        $(this).css({
                            zIndex: 5
                        });
                        $('.' + option.container, elem).css({
                            background: ''
                        });
                        loaded = true;
                    });
                });
            } else {
                control.children(':eq(' + start + ')').fadeIn(option.fadeSpeed, option.fadeEasing, function () {
                    loaded = true;
                });
            }
            if (option.bigTarget) {
                control.children().css({
                    cursor: 'pointer'
                });
                control.children().click(function () {
                    animate('next', effect);
                    return false;
                });
            }
            if (option.hoverPause && option.play) {
                control.bind('mouseover', function () {
                    stop();
                });
                control.bind('mouseleave', function () {
                    pause();
                });
            }
            if (option.generateNextPrev) {
                var urlstring=document.location.href;
								if (urlstring.search("espanol") == -1) {
                $('.' + option.container, elem).after('<a href="#" class="' + option.prev + '"><img src="/sysimages/tile-slider-english-previous.gif" alt="View Previous Tile" border="0" width="81" height="15" /></a>');
                $('.' + option.prev, elem).after('<a href="#" class="' + option.next + '"><img src="/sysimages/tile-slider-english-next.gif" alt="View Next Tile" border="0" width="78" height="15" /></a>');
                }
                else {
                	$('.' + option.container, elem).after('<a href="#" class="' + option.prev + '"><img src="/sysimages/tile-slider-spanish-previous.gif" alt="View Previous Tile" border="0" width="81" height="15" /></a>');
                $('.' + option.prev, elem).after('<a href="#" class="' + option.next + '"><img src="/sysimages/tile-slider-spanish-next.gif" alt="View Next Tile" border="0" width="78" height="15" /></a>');
                }
                
                
            }
            $('.' + option.next, elem).click(function (e) {
                e.preventDefault();
                if (option.play) {
                    pause();
                }
                animate('next', effect);
            });
            $('.' + option.prev, elem).click(function (e) {
                e.preventDefault();
                if (option.play) {
                    pause();
                }
                animate('prev', effect);
            });
            if (option.generatePagination) {
                elem.append('<ul class=' + option.paginationClass + '></ul>');
                control.children().each(function () {
                    $('.' + option.paginationClass, elem).append('<li><a href="#' + number + '">' + (number + 1) + '</a></li>');
                    number++;
                });
            } else {
                $('.' + option.paginationClass + ' li a', elem).each(function () {
                    $(this).attr('href', '#' + number);
                    number++;
                });
            }
            $('.' + option.paginationClass + ' li:eq(' + start + ')', elem).addClass('current');
            $('.' + option.paginationClass + ' li a', elem).click(function () {
                if (option.play) {
                    pause();
                }
                clicked = $(this).attr('href').match('[^#/]+$');
                if (current != clicked) {
                    animate('pagination', paginationEffect, clicked);
                }
                return false;
            });
            $('a.link', elem).click(function () {
                if (option.play) {
                    pause();
                }
                clicked = $(this).attr('href').match('[^#/]+$') - 1;
                if (current != clicked) {
                    animate('pagination', paginationEffect, clicked);
                }
                return false;
            });
            if (option.play) {
                playInterval = setInterval(function () {
                    animate('next', effect);
                }, option.play);
                elem.data('interval', playInterval);
            }
        });
    };
    $.fn.slides.option = {
        preload: false,
        preloadImage: '/img/loading.gif',
        container: 'slides_container',
        generateNextPrev: false,
        next: 'next',
        prev: 'prev',
        pagination: true,
        generatePagination: true,
        paginationClass: 'pagination',
        fadeSpeed: 350,
        fadeEasing: '',
        slideSpeed: 350,
        slideEasing: '',
        start: 1,
        effect: 'slide',
        crossfade: false,
        randomize: false,
        play: 0,
        pause: 0,
        hoverPause: false,
        autoHeight: false,
        autoHeightSpeed: 350,
        bigTarget: false,
        animationStart: function () {},
        animationComplete: function () {}
    };
    $.fn.randomize = function (callback) {
        function randomizeOrder() {
            return (Math.round(Math.random()) - 0.5);
        }
        return ($(this).each(function () {
            var $this = $(this);
            var $children = $this.children();
            var childCount = $children.length;
            if (childCount > 1) {
                $children.hide();
                var indices = [];
                for (i = 0; i < childCount; i++) {
                    indices[indices.length] = i;
                }
                indices = indices.sort(randomizeOrder);
                $.each(indices, function (j, k) {
                    var $child = $children.eq(k);
                    var $clone = $child.clone(true);
                    $clone.show().appendTo($this);
                    if (callback !== undefined) {
                        callback($child, $clone);
                    }
                    $child.remove();
                });
            }
        }));
    };
})(jQuery); /* END Home Page Tile Slider */