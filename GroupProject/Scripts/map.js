﻿import { start, end } from "./autocomplete.js";

let placesInTrip = [];
let placesList = document.querySelector("#added-places");

let button = document.querySelector('.start-button');
button.addEventListener("click", async (e) => initMap(e));

export async function initMap(event) {
    event.preventDefault();

    let directionRenderer = new google.maps.DirectionsRenderer();

    let map = new google.maps.Map(document.getElementById("map"));
    directionRenderer.setMap(map);

    let directions = await GetDirections(start, end);

    let places = await GetPlaces(directions);

    console.log(directions);
    directionRenderer.setDirections(directions);

    let markers = await SetMarkers(map, places);

    const markerCluster = new markerClusterer.MarkerClusterer({ map, markers });
}

async function GetDirections(startPoint, endPoint) {
    let directionService = new google.maps.DirectionsService();

    let startCoordinates = new google.maps.LatLng(
        startPoint.coordinates.latitude,
        startPoint.coordinates.longitude
    );
    let endCoordinates = new google.maps.LatLng(
        endPoint.coordinates.latitude,
        endPoint.coordinates.longitude
    );

    let request = {
        origin: startCoordinates,
        destination: endCoordinates,
        travelMode: "DRIVING",
    };

    let directions;

    await directionService.route(request, (result, status) => {
        if (status == "OK") {
            directions = result;
        }
    });

    return directions;
}

async function GetPlaces(directions) {
    let overview_path = directions.routes[0].overview_path.map((c) => {
        let coordinate = c.toJSON();
        return { longitude: coordinate.lng, latitude: coordinate.lat };
    });
    console.log(overview_path);
    let start = Date.now();

     let response = await fetch(
       "https://localhost:44397/api/places/getplacesalongpath",
       {
         method: "POST",
         headers: {
           "Content-Type": "application/json;charset=utf-8",
         },
         body: JSON.stringify(overview_path),
       }
     );

     let result = await response.json();

     let end = Date.now() - start;
     console.log(end);

     console.log(result);

    return result;
}

async function SetMarkers(map, places) {

    const infoWindow = new google.maps.InfoWindow();

    let markers = [];

    places.forEach((place) => {
        const marker = new google.maps.Marker({
            position: { lat: place.point.lat, lng: place.point.lon },
            // position: { lat: place.latitude, lng: place.longitude }, // plot overview_path
            map,
            title: place.name,
        });

        marker.addListener("click", async () => {
            const placeInfo = await GetPlaceInfo(place);
            let infoWindowElement = CreateWindowInfoElement(placeInfo);
            console.log(infoWindowElement);


            infoWindow.close();
            infoWindow.setContent(marker.getTitle());
            infoWindow.open(marker.getMap(), marker);
        });

        markers.push(marker);
    });

    return markers;
}

async function GetPlaceInfo(place) {
    let url = `https://api.opentripmap.com/0.1/en/places/xid/${place.xid}?apikey=5ae2e3f221c38a28845f05b6068096737a6bd1b9a215367ca1d1bd33`;

    let placeInfo = await newCache.match(url);

    if (!placeInfo) {
        let response = await fetch(url);
        console.log("Not cached");
        if (response.ok) {
            placeInfo = response;
            newCache.put(url, response.clone());
        } else {
            alert("HTTP-Error: " + response.status);
        }
    }

    return placeInfo.json();
}

function CreateWindowInfoElement(place) {

    let map = document.querySelector("#map");

    let container = document.createElement("div");
    map.append(container);

    let image = document.createElement("img");
    image.src = place.preview.source;
    container.append(image);

    let title = document.createElement("p");
    title.textContent = place.name;
    container.append(title);

    let description = document.createElement("p");
    description.innerHTML = place.wikipedia_extracts.html;
    container.append(description);

    let kindsList = document.createElement("ul");
    let kindsItems = kindsParsed(place.kinds).map((kind) => {
        let listItem = document.createElement("li");
        listItem.textContent = kind;

        return listItem;
    });
    kindsItems.forEach((kind) => {
        kindsList.append(kind);
    });
    container.append(kindsList);

    let addBtn = document.createElement("button");
    addBtn.textContent = "Add to Trip";
    addBtn.addEventListener("click", () => {
        if (!placesInTrip.find((p) => p.xid === place.xid))
            placesInTrip.push(place);

        let placeItem = document.createElement("li");
        placeItem.textContent = place.name;
        placesList.append(placeItem);

        console.log(placesInTrip);
    });
    container.append(addBtn);

    let anchor = document.createElement("a");
    anchor.innerHTML = `<a href="https://www.google.com/search?q=${place.name}" target="_blank">Open in Google</a>`;
    container.append(anchor);

    return container;
}

let kindsParsed = (kinds) => {
    let kindsSplit = kinds.split(",");

    let parsed = kindsSplit.map((kind) => {
        let capitalized = kind.charAt(0).toUpperCase() + kind.slice(1);

        let split = capitalized.split("_");

        return split.join(" ");
    });

    return parsed;
};
