console.log("Lets get tis party started");

let searchField = document.getElementById("site-search");
let cards = document.getElementsByClassName('card');

function filter() {

    let searchString = searchField.value;

    if (searchString == "")
        searchString == ".*";

    for (let index = 0; index < cards.length; index++) {
        // console.log(cards[index]);
        if (cards[index].getElementsByTagName('input')[0].value.search(new RegExp(searchString, "i")) != -1) {
            cards[index].style.display = 'inline-block';
            // console.log("visa");
        }
        else {

            cards[index].style.display = 'none';
            // console.log("dölj");
        }
    }
}
// cards[0].getElementsByName('searchTags')
// let searchTags = cards[0].getElementsByTagName('input')[0].value;
// let array = cards.map(c => ({ id: c.id, searchTags: c.value }));
// let showIDs = [];
// for (var cards of cards) {
//     showIDs.push()
//     console.log(searchTag)
// }


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
