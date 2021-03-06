/*
* Template Name: Unify - Responsive Bootstrap Template
* Author: @htmlstream
* Website: http://htmlstream.com
*/

var App = function () {
	// We extend jQuery by method hasAttr
	$.fn.hasAttr = function(name) {
	  return this.attr(name) !== undefined;
	};

	// Fixed Header
	function handleHeader() {
		jQuery(window).scroll(function() {
		  if (jQuery(window).scrollTop() > 100) {
			jQuery('.header-fixed .header-sticky').addClass('header-fixed-shrink');
		  } else {
			jQuery('.header-fixed .header-sticky').removeClass('header-fixed-shrink');
		  }
		});
	}

	// Header Mega Menu
	function handleMegaMenu() {
		jQuery(document).on('click', '.mega-menu .dropdown-menu', function(e) {
			e.stopPropagation();
		})
	}

	// Search Box (Header)
	function handleSearch() {
		jQuery('.search').on("click", function () {
			if(jQuery('.search-btn').hasClass('fa-search')){
			    jQuery('.search-open').fadeIn(100);
				jQuery('.search-btn').removeClass('fa-search');
				jQuery('.search-btn').addClass('fa-times');				
			} else {
			    jQuery('.search-open').fadeOut(500);
				jQuery('.search-btn').addClass('fa-search');
				jQuery('.search-btn').removeClass('fa-times');
			}
            jQuery("#criteria").focus();
		});
	}

	// Sidebar Navigation Toggle
	function handleToggle() {
		jQuery('.list-toggle').on('click', function() {
			jQuery(this).toggleClass('active');
		});
	}

	// Equal Height Columns
	function handleEqualHeightColumns() {
		var EqualHeightColumns = function () {
			$(".equal-height-columns").each(function() {
				heights = [];
				$(".equal-height-column", this).each(function() {
					$(this).removeAttr("style");
					heights.push($(this).height()); // write column's heights to the array
				});
				$(".equal-height-column", this).height(Math.max.apply(Math, heights)); //find and set max
			});
		}

		EqualHeightColumns();
		$(window).resize(function() {
			EqualHeightColumns();
		});
		$(window).load(function() {
			EqualHeightColumns();
		});
	}

	// Equal Height Image-Columns
	function handleEqualHeightColumns__Images() {
		var EqualHeightColumns__Images = function () {
			$('.equal-height-columns-v2').each(function() {
				var heights = [];
				$('.equal-height-column-v2', this).each(function() {
					$(this).removeAttr('style');
					heights.push($(this).height()); // Write column's heights to the array
				});
				$('.equal-height-column-v2', this).height(Math.max.apply(Math, heights)); // Find and set max

				$('.equal-height-column-v2', this).each(function() {
					if ($(this).hasAttr('data-image-src')) {
						$(this).css('background', 'url('+$(this).attr('data-image-src')+') no-repeat scroll 50% 0 / cover');
					}
				});
			});
		}
    $('.equal-height-columns-v2').ready(function() {
      EqualHeightColumns__Images();
    });
		$(window).resize(function() {
			EqualHeightColumns__Images();
		});
	}

	// Full Screen
	var handleFullscreen = function() {
		var WindowHeight = $(window).height();
		var HeaderHeight = 0;

		if ($(document.body).hasClass("promo-padding-top")) {
		  HeaderHeight = $(".header").height();
		} else {
		  HeaderHeight = 0;
		}

		$(".fullheight").css("height", WindowHeight - HeaderHeight);

		$(window).resize(function() {
		  var WindowHeight = $(window).height();
		  $(".fullheight").css("height", WindowHeight - HeaderHeight);
		});
	}

	// Align Middle
	var handleValignMiddle = function() {
		$(".valign__middle").each(function() {
		  $(this).css("padding-top", $(this).parent().height() / 2 - $(this).height() / 2);
		});
		$(window).resize(function() {
		  $(".valign__middle").each(function() {
			$(this).css("padding-top", $(this).parent().height() / 2 - $(this).height() / 2);
		  });
		});
	}

	// Hover Selector
	function handleHoverSelector() {
		// $('.hoverSelector').on('hover', function(e) {
		// 	$('.hoverSelectorBlock', this).toggleClass('show');
		// 	e.stopPropagation();
		// });
	    $('.hoverSelector').on('click', function(e) {
	      if (jQuery(this).children('ul').hasClass('languages')) {
	        if (jQuery(this).children('ul').hasClass('languages-visible')) {
	          jQuery(this).children('.languages').slideUp();
	          jQuery(this).children('.languages').removeClass('languages-visible');
	        } else {
	          jQuery(this).children('.languages').slideDown();
	          jQuery(this).children('.languages').addClass('languages-visible');
	        }
	      }
	      //e.preventDefault();
	    });
	}

	// Bootstrap Tooltips and Popovers
	function handleBootstrap() {
		/* Bootstrap Carousel */
		jQuery('.carousel').carousel({
			interval: 15000,
			pause: 'hover'
		});

		/* Tooltips */
		jQuery('.tooltips').tooltip();
		jQuery('.tooltips-show').tooltip('show');
		jQuery('.tooltips-hide').tooltip('hide');
		jQuery('.tooltips-toggle').tooltip('toggle');
		jQuery('.tooltips-destroy').tooltip('destroy');

		/* Popovers */
		jQuery('.popovers').popover();
		jQuery('.popovers-show').popover('show');
		jQuery('.popovers-hide').popover('hide');
		jQuery('.popovers-toggle').popover('toggle');
		jQuery('.popovers-destroy').popover('destroy');
	}

	return {
		init: function () {
			handleBootstrap();
			handleSearch();
			handleToggle();
			handleHeader();
			handleMegaMenu();
			handleHoverSelector();
			handleFullscreen();
			handleValignMiddle();
			handleEqualHeightColumns();
			handleEqualHeightColumns__Images();
		},

		// Counters
		initCounter: function () {
			jQuery('.counter').counterUp({
				delay: 10,
				time: 1000
			});
		},

		// Parallax Backgrounds
		initParallaxBg: function () {
			jQuery(window).load(function() {
				jQuery('.parallaxBg').parallax("50%", 0.2);
				jQuery('.parallaxBg1').parallax("50%", 0.4);
			});
		},

		// Scroll Bar
		initScrollBar: function () {
			jQuery('.mCustomScrollbar').mCustomScrollbar({
				theme:"minimal",
				scrollInertia: 200,
				scrollEasing: "linear"
			});
		},

		// Sidebar Menu Dropdown
		initSidebarMenuDropdown: function() {
		  function SidebarMenuDropdown() {
			jQuery('.header-v7 .dropdown-toggle').on('click', function() {
			  jQuery('.header-v7 .dropdown-menu').stop(true, false).slideUp();
			  jQuery('.header-v7 .dropdown').removeClass('open');

			  if (jQuery(this).siblings('.dropdown-menu').is(":hidden") == true) {
				jQuery(this).siblings('.dropdown-menu').stop(true, false).slideDown();
				jQuery(this).parents('.dropdown').addClass('open');
			  }
			});
		  }
		  SidebarMenuDropdown();
		},

		// Animate Dropdown
		initAnimateDropdown: function() {
		  function MenuMode() {
			jQuery('.dropdown').on('show.bs.dropdown', function() {
			  jQuery(this).find('.dropdown-menu').first().stop(true, true).slideDown();
			});
			jQuery('.dropdown').on('hide.bs.dropdown', function() {
			  jQuery(this).find('.dropdown-menu').first().stop(true, true).slideUp();
			});
		  }

		  jQuery(window).resize(function() {
			if (jQuery(window).width() > 768) {
			  MenuMode();
			}
		  });

		  if (jQuery(window).width() > 768) {
			MenuMode();
		  }
		},
	};
}();
