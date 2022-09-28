export default async function GetDirections(startPoint, endPoint) {
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