//FUNCTION CALL                                                                
window.addEventListener('DOMContentLoaded', (event) =>{         // When this content has loaded --> run even getVisitCount
    getVisitCount();
})

const functionApi = '';

//CREATE FUNCTION
const getVisitCount = () => {
    let count = 30; Promise<Response>                           //count variable temporarily set to 30 
    fetch(functionApi).then(repsonse => {                       //fetch function api data, grab response, return JSON
        return Response.json()
    }).then(response => {
        console.log("Website called function API.");            //log response to console for debugging purposes
        count = response.count;                                 //count variable set to json resonse data
        document.getElementById("counter").innerText = count;   //set counter innner text to count variable 
    }).catch(function(error){ 
        console.log(error);                                     //grab any erros & log message to console for debugging
    });
    return count;
}