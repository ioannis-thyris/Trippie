let placeBtns = Array.from(document.querySelectorAll('.placeBtn'));

placeBtns.forEach(btn => {
    let placesDiv = btn.parentElement.nextElementSibling;

    btn.addEventListener('click', () => {
        placesDiv.hidden = !placesDiv.hidden;
    });
});



let googleBtns = Array.from(document.querySelectorAll('.googleBtn'));
googleBtns.forEach(btn => {
    btn.addEventListener('click', (e) => {
        let container = e.target.parentElement.parentElement.parentElement;

        let startEle = container.querySelector('.startDiv');
        let endEle = container.querySelector('.endDiv');

        let placeEles = Array.from(container.querySelectorAll('.placeDiv'));
        console.log(container);
        console.log(placeEles);
        let places = placeEles.map(ele => {
            console.log('mpika');
            let latitude = ele.getAttribute('lat');
            let longitude = ele.getAttribute('lon');
            console.log(latitude);
            return {
                lat: +latitude.replaceAll(',', '.'),
                lon: +longitude.replaceAll(',', '.')
            }
        });

        let startLatitude = startEle.getAttribute('startLat');
        let startLongitude = startEle.getAttribute('startLon');
        let endLatitude = endEle.getAttribute('endLat');
        let endLongitude = endEle.getAttribute('endLon');

        let tripDto = {
            startLat: +startLatitude.replaceAll(',', '.'),
            startLon: +startLongitude.replaceAll(',', '.'),
            endLat: +endLatitude.replaceAll(',', '.'),
            endLon: +endLongitude.replaceAll(',', '.'),
            waypoints: places
        };
        console.log(tripDto);
        OpenTripInGoogleMaps(tripDto);

    });
});






function OpenTripInGoogleMaps(tripDto) {
    fetch('https://localhost:44397/Trip/RedirectToGoogleMaps', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(tripDto)
    }).then((response) => response.json())
        .then((url) => {
            window.open(url.googleMapsUrl, '_blank').focus();
        })
}
