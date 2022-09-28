import { GetPlaceInfo } from "./places.js";

export let placesInTrip = [];

export async function SetMarkers(map, places) {

    const infoWindow = new google.maps.InfoWindow();

    let markers = [];

    places.forEach((place) => {
        const marker = new google.maps.Marker({
            position: { lat: place.point.lat, lng: place.point.lon },
            map,
            title: place.name,
        });

        marker.addListener("click", async () => {
            const placeInfo = await GetPlaceInfo(place);
            let infoWindowElement = CreateWindowInfoElement(placeInfo);

            infoWindow.close();
            infoWindow.setContent(infoWindowElement);
            infoWindow.open(marker.getMap(), marker);
        });

        markers.push(marker);
    });

    return markers;
}

function CreateWindowInfoElement(place) {

    let map = document.querySelector("#map");

    let container = document.createElement("div");
    container.classList.add('infoWindow');
    map.append(container);

    let image = document.createElement("img");
    image.src = place.preview.source;
    container.append(image);

    let headerContainer = document.createElement("div");
    headerContainer.classList.add("header-container");

    let title = document.createElement("p");
    title.classList.add('h4')
    title.textContent = place.name;
    headerContainer.append(title);

    let anchor = document.createElement("a");
    anchor.innerHTML = `<a href="https://www.google.com/search?q=${place.name}" target="_blank">Open in Google</a>`;
    headerContainer.append(anchor);

    container.append(headerContainer);

    let description = document.createElement("p");
    description.innerHTML = place.wikipedia_extracts.html;
    container.append(description);

    let addBtnContainer = document.createElement("div");
    addBtnContainer.classList.add("d-flex");
    addBtnContainer.classList.add("justify-content-center");

    let addBtn = document.createElement("button");

    addBtn.classList.add('btn');
    addBtn.classList.add('btn-dark');

    addBtn.setAttribute('type', 'button');
    addBtn.textContent = "Add to Trip";
    addBtn.addEventListener("click", () => {
        if (!placesInTrip.find((p) => p.xid === place.xid))
            placesInTrip.push(place);

        if (!PlaceRowInList(place)) {
            CreatePlaceRow(place);
        }

        let addTripBtn = document.querySelector('#add-trip');
        addTripBtn.disabled = false;
    });

    addBtnContainer.append(addBtn);

    container.append(addBtnContainer);

    return container;
}

function CreatePlaceRow(place) {
    let placeRow = document.createElement("li");
    placeRow.setAttribute('key', place.xid);
    placeRow.classList.add('list-group-item');
    placeRow.classList.add('d-flex');
    placeRow.classList.add('justify-content-between');

    let placeInfo = document.createElement("div");
    placeInfo.textContent = place.name;

    let deleteBtn = document.createElement("span");
    deleteBtn.innerHTML = '<button class="delete-place"><i class="fa fa-trash" aria-hidden="true"></i></button>'
    deleteBtn.addEventListener('click', () => {
        let row = deleteBtn.parentElement;
        let id = row.getAttribute('key');
        row.remove();

        let index = placesInTrip.map(p => p.xid)
            .indexOf(id);

        placesInTrip.splice(index, 1);


        let addTripBtn = document.querySelector('#add-trip');

        if (placesInTrip.length == 0)
            addTripBtn.disabled = true;
    });

    placeRow.append(placeInfo);
    placeRow.append(deleteBtn);

    let ol = document.querySelector('#places-list');

    ol.append(placeRow);
}

function PlaceRowInList(place) {
    let list = document.querySelector('#places-list')

    let rows = Array.from(list.children);

    if (rows.length == 0)
        return false;

    let ids = rows.map(r => r.getAttribute('key'));

    return ids.some(id => place.xid === id);
}