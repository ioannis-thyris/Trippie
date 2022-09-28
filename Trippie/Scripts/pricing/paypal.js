paypal.Buttons({
    // Sets up the transaction when a payment button is clicked
    createOrder: (data, actions) => {
        return fetch("https://localhost:44397/Pricing/CreateOrder", {
            method: "post",
            // use the "body" param to optionally pass additional order information
            // like product ids or amount
        })
            .then((response) => response.json())
            .then((order) => order.id);
    },
    // Finalize the transaction after payer approval
    onApprove: (data, actions) => {
        debugger;
        return fetch(`https://localhost:44397/Pricing/CapturePayment?orderId=${data.orderID}`, {
            method: "post",
            body: JSON.stringify(data)
        })
            .then((response) => response.json())
            .then((orderData) => {
                window.location.href = orderData.redirectToUrl;
            });
    }
}).render('#paypal-button-container');