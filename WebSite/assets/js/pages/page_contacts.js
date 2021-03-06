var ContactPage = function () {

    return {
        
    	//Basic Map
        initMap: function () {
			var map;
			$(document).ready(function(){
			  map = new GMaps({
				div: '#map',
				scrollwheel: false,				
				lat: 42.130294,
				lng: 24.7787533,
			  });
			  
			  var marker = map.addMarker({
			      lat: 42.130294,
			      lng: 24.7787533,
	            title: 'Company, Inc.'
		       });
			});
        },

        //Panorama Map
        initPanorama: function () {
		    var panorama;
		    $(document).ready(function(){
		      panorama = GMaps.createPanorama({
		        el: '#panorama',
		        lat : 42.130294,
		        lng : 24.7787533,
		      });
		    });
		}        

    };
}();

