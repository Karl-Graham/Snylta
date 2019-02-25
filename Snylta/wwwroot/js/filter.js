console.log("Lets get tis party started");

let searchTags = document.getElementsByName('searchTags');
console.log(searchTags);
let showIDs = [];
for (var searchTag of searchTags) {
    console.log(searchTag)
    showIDs
    // checkbox.checked = true;
}


// let array = cards.map(c => ({ id: c.id, searchTags: c.value }));
// console.log(array);



// class Button extends React.Component {
//     handleClick = () => {
//         this.props.ContactRolifyAPI(this.props.action, this.props.method, this.props.body);
//     }
//     render() {
//         return (<button type="button" className="btn btn-primary btn-lg" style={{ margin: 10 }} onClick={this.handleClick}>{this.props.title}</button>);
//     }
// }

// class App extends React.Component {
//     constructor(props) {
//         super(props);
//         // Don't call this.setState() here!
//         this.state = {
//             message: "Rolify",

//             controlButtons: [
//                 {
//                     title: 'Play',
//                     action: 'play',
//                     method: 'PUT',
//                 },
//                 {
//                     title: 'Pause',
//                     action: 'pause',
//                     method: 'PUT',
//                 }
//             ],

//             soundButtons: []
//         }
//     };
//     async componentDidMount() {

//         let resp = await fetch(`API/Things`);
//         // if ((resp.status > 200 && resp.status < 300) == false) {
//         //     alert("Ingen kontakt");
//         // }
//         let json = await resp.json();

//         console.log(json.map(b => ({ title: b.name, action: 'Play', body: { "context_uri": b.context_uri } })))

//         this.setState({ soundButtons: json.map(b => ({ title: b.name, action: 'Play', body: { "context_uri": b.context_uri } })) });
//     }

//     // ContactRolifyAPI = async (action, method, body) => {
//     //     let resp = await fetch(
//     //         `https://api.spotify.com/v1/me/player/${action}`,
//     //         {
//     //             method: method,
//     //             headers: {
//     //                 Authorization: 'Bearer ' + access_token,
//     //                 'Content-Type': 'application/json'
//     //             },
//     //             body: JSON.stringify(
//     //                 body
//     //             )
//     //         }
//     //     );

//     //     if ((resp.status > 200 && resp.status < 300) == false) {
//     //         alert("Ingen kontakt");
//     //     }

//     // };

//     render() {
//         return (
//             <React.Fragment>
//                 <h1>{this.state.message}</h1>
//                 <hr></hr>
//                 <h3>Controls</h3>
//                 {this.state.controlButtons.map(b => <Button {...b} ContactRolifyAPI={this.ContactRolifyAPI}></Button>)}
//                 <hr></hr>
//                 <h3>Sounds</h3>
//                 {this.state.soundButtons.map(b => <Button {...b} ContactRolifyAPI={this.ContactRolifyAPI}></Button>)}
//             </React.Fragment>
//         )
//     }
// }
