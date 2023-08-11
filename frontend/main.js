//FUNCTION CALL                                                                
window.addEventListener('DOMContentLoaded', (event) =>{         // When this content has loaded --> run even getVisitCount
    getVisitCount();
})

const functionApi = 'http://localhost:7071/api/GetResumeCounter';

//CREATE FUNCTION
const getVisitCount = () => {
    fetch(functionApi)
        .then(response => response.json()) // Parse response as JSON
        .then(data => {
            console.log("Website called function API.");
            const count = data.count; // Get the count value from the JSON data
            document.getElementById("counter").innerText = count;
        })
        .catch(error => {
            console.log(error);
        });
};
