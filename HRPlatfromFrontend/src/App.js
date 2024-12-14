import './App.css';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Profile from './profile';
import TaskContainer from './TaskContainer';

import CheckEvents from './components/checkEvents';
import AddWork from './components/AddWork';
import Login from './components/Login';
import Meeting from './components/meeting';
import Sidebar from './component/sidebar';
import AnonymBox from './AnonymBox';
import  ViewBoxSection from './ViewBoxSection';
import AdminFunctionalities from "./components/AdminFunctionalities";
import Scan from './components/scan';
import Confirm from './components/confirm';
import AddBlame from './components/AddBlame';
import 'bootstrap/dist/css/bootstrap.min.css';
import {useEffect, useState} from "react";
import AdminRouteGuard from "./components/AdminRouteGuard";
import UserRouteGuard from "./components/UserRouteGuard";
import checkEvents from "./components/checkEvents";



function App() {

  

  return (
      <>
          <Router >
              <Routes>
                  <Route path='/login' element={<Login></Login>}> </Route>
                  </Routes>

          </Router>

          <Router>


                      <Routes>

                          <Route path="/events" element={<UserRouteGuard component={<CheckEvents></CheckEvents>     }></UserRouteGuard>}></Route>
                          <Route path="/functionalities" element={<AdminRouteGuard component={<AdminFunctionalities></AdminFunctionalities>}></AdminRouteGuard>}></Route>
                          <Route path='/task' element={<UserRouteGuard component={<TaskContainer/>     }></UserRouteGuard>}   />
                          <Route path='/meeting'  element={<UserRouteGuard component={<Meeting />    }></UserRouteGuard>} />
                          <Route path='/anonymbox' element={<UserRouteGuard component={<AnonymBox/>   }></UserRouteGuard>}/>
                          <Route path='/profile' element={<UserRouteGuard component={<Profile />  }></UserRouteGuard>} />
                          <Route path="/scan/:meetingId" element={<UserRouteGuard component={<Scan/>  }></UserRouteGuard>} />
                          <Route path='/comments' element={<UserRouteGuard component={<ViewBoxSection/> }></UserRouteGuard>} />
                          <Route path='/blames' element={<UserRouteGuard component={<AddBlame/>   }></UserRouteGuard>} />
                      </Routes>


          </Router>

      </>
  );
}

export default App;
