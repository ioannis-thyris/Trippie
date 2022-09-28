import { start, end } from "./autocomplete.js";
import GetCheckedTypes from "./types.js"
import GetDirections from "./directions.js";
import { GetPlaces } from "./places.js";
import { SetMarkers, placesInTrip } from "./markers.js";


export async function initMap(event) {
    event.preventDefault();

    loading.hidden = false;

    let directionRenderer = new google.maps.DirectionsRenderer();

    let map = new google.maps.Map(document.getElementById("map"));

    directionRenderer.setMap(map);

    let directions = await GetDirections(start, end);

    let places = await GetPlaces(directions);

    console.log(places);
    directionRenderer.setDirections(directions);

    let markers = await SetMarkers(map, places);

    const markerCluster = new markerClusterer.MarkerClusterer({ map, markers });

    mapOverlay.hidden = true;
    loading.hidden = true;
}

const mapOverlay = document.querySelector('.map-overlay');
const loading = document.querySelector('.spinner-border');

let startBtn = document.querySelector('#start-button');
startBtn.addEventListener("click", (e) => {
    let checked = GetCheckedTypes();
    console.log(checked);
    if (start && end && checked.length) {
        initMap(e)
    } else {
        alert("Start, destination and place types must be set.");
    }
})

let inputForm = document.querySelector('#input-from');
inputForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    let createTripBtn = e.target.querySelector('button[type=submit]');

    let tripDto = GetTripDto();

    let isRegistered = createTripBtn.getAttribute('isregistered') == 'true' ? true : false;

    if (isRegistered) {
        console.log('Store');
        StoreTripInDB(tripDto);
    } else {
        console.log('Maps');
        OpenTripInGoogleMaps(tripDto);
    }
});


function OpenTripInGoogleMaps(tripDto) {
    fetch('https://localhost:44397/Trip/RedirectToGoogleMapsUnregistered', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(tripDto)
    }).then((response) => response.json())
        .then((url) => {
            window.location.href = url.googleMapsUrl;
        })
}


function StoreTripInDB(tripDto) {
    fetch('https://localhost:44397/Trip/CreateTrip', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(tripDto)
    }).then((response) => response.json())
        .then((url) => {
            window.location.href = url.redirectToUrl;
        })
}

function GetTripDto() {
    let types = GetCheckedTypes();

    let placeDtos = placesInTrip.map(p => {
        return {
            xid: p.xid,
            name: p.name,
            rate: p.rate,
            imageUrl: p.preview.source,
            info: p.wikipedia_extracts.text,
            latitude: p.point.lat,
            longitude: p.point.lon
        }
    });

    let startDto = {
        geonameID: start.geonameID,
        name: start.asciiName,
        country: start.countryNameEN,
        longitude: start.coordinates.longitude,
        latitude: start.coordinates.latitude
    }

    let endDto = {
        geonameID: end.geonameID,
        name: end.asciiName,
        country: end.countryNameEN,
        longitude: end.coordinates.longitude,
        latitude: end.coordinates.latitude
    }

    let tripDto = {
        start: startDto,
        end: endDto,
        creationDate: new Date().toJSON(),
        chosenPlaceTypes: [...types],
        places: [...placeDtos]
    }

    return tripDto;
}

