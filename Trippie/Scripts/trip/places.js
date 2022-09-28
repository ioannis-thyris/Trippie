import GetCheckedTypes from "./types.js"
import { radiusValue, pointsValue } from "./sliders.js";

const newCache = await caches.open("new-cache");

async function GetPlaces(directions) {
    let overview_path = directions.routes[0].overview_path.map((c) => {
        let coordinate = c.toJSON();
        return { longitude: coordinate.lng, latitude: coordinate.lat };
    });

    let types = GetCheckedTypes();

    let dto = {
        pathOverview: overview_path,
        radius: radiusValue * 1000,
        pointsAlongPath: pointsValue,
        placeTypes: types,
    }

    let response = await fetch(
        "https://localhost:44397/api/places/getplacesalongpath",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json;charset=utf-8",
            },
            body: JSON.stringify(dto),
        }
    );

    let result = await response.json();

    return result;
}

async function GetPlaceInfo(place) {
    let url = `https://api.opentripmap.com/0.1/en/places/xid/${place.xid}?apikey=5ae2e3f221c38a28845f05b6068096737a6bd1b9a215367ca1d1bd33`;

    let placeInfo = await newCache.match(url);

    if (!placeInfo) {
        let response = await fetch(url);

        if (response.ok) {
            placeInfo = response;
            newCache.put(url, response.clone());
        } else {
            alert("HTTP-Error: " + response.status);
        }
    }

    return placeInfo.json();
}

export {
    GetPlaces,
    GetPlaceInfo
}