export default function GetCheckedTypes() {
    let checkboxes = Array.from(document.querySelectorAll('input[type=checkbox'));
    let checked = [];

    checkboxes.forEach(c => {
        if (c.checked)
            checked.push(+c.value);
    });

    return checked;
}