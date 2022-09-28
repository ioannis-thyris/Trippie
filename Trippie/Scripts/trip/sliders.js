let radiusInput = document.querySelector('input[name=radius]');
let radiusValue = +radiusInput.value;

let radiusText = document.querySelector('input[name=radius] ~ p');
radiusText.textContent = radiusInput.value;

radiusInput.addEventListener('input', () => {
    radiusText.textContent = radiusInput.value;
    radiusValue = +radiusInput.value;
});


let pointsInput = document.querySelector('input[name=points]');
let pointsValue = +pointsInput.value;

let pointsText = document.querySelector('input[name=points] ~ p');
pointsText.textContent = pointsInput.value;

pointsInput.addEventListener('input', () => {
    pointsText.textContent = pointsInput.value;
    pointsValue = pointsInput.value;
    console.log(+pointsValue);
    console.log(typeof +pointsValue);
});


export {
    radiusValue,
    pointsValue
}