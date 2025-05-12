import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import { useAuth } from '../../Context/AuthContext';

const DoctorDashboard = () => {
    const { userId } = useAuth();
    const [activeTab, setActiveTab] = useState('Profile');
    const [doctor, setDoctor] = useState(null);
    const [photoUrl, setPhotoUrl] = useState('');
    const [cvUrl, setCvUrl] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [appointments, setAppointments] = useState([]);
    const [diagnostic, setDiagnostic] = useState("");
    const [tratament, setTratament] = useState("");
    const [saving, setSaving] = useState(false);
    const [searchPacientfullName, setSearchPacientfullName] = useState("");
    const [patients, setPatients] = useState([]);



    useEffect(() => {
        const fetchDoctorData = async () => {
            try {
                const res = await axios.get(`http://localhost:5000/doctor/user/${userId}`);
                setDoctor(res.data);

                const photoRes = await axios.get(`http://localhost:5000/doctor/${res.data.userId}/profile-image`, {
                    responseType: 'blob',
                });
                setPhotoUrl(URL.createObjectURL(photoRes.data));

                const cvRes = await axios.get(`http://localhost:5000/doctor/${res.data.userId}/cv`, {
                    responseType: 'blob',
                });
                const blob = new Blob([cvRes.data], { type: 'application/pdf' });
                setCvUrl(URL.createObjectURL(blob));
            } catch (err) {
                console.error('Fetch error:', err);
                setError('Failed to load doctor data.');
            } finally {
                setLoading(false);
            }
        };

        fetchDoctorData();
    }, [userId]);

    useEffect(() => {
        const fetchAppointments = async () => {
            try {
                const res = await axios.get(`http://localhost:5000/appointment/search?doctorUserId=${userId}`);
                setAppointments(res.data);
            } catch (err) {
                console.error('Failed to fetch appointments:', err);
            }
        };
        const fetchOwnPacients = async () => {
            try {
                const res = await axios.get(`http://localhost:5000/pacient/searchbymedic?doctorUserId=${userId}`);
                setPatients(res.data);
            } catch (err) {
                console.error('Failed to fetch appointments:', err);
            }
        };


        if (activeTab === 'Appointments') {
            fetchAppointments();
        }
        if (activeTab === 'Patients') {
            fetchOwnPacients();
        }
    }, [activeTab, userId]);

    const handleSearchPacient = async () => {
        try {
            if (!searchPacientfullName.trim()) {
                const response = await axios.get(`http://localhost:5000/pacient/searchbymedic?doctorUserId=${userId}`);
                setPatients(response.data);
            }
            else {
                const response = await axios.get(`http://localhost:5000/pacient/searchbymedic?doctorUserId=${userId}&name=${searchPacientfullName}`);
                setPatients(response.data);
            }
        } catch (err) {
            console.error("Error fetching patients:", err);
        }
    };

    const handleSearch = async () => {
        try {
            const response = await axios.get(`http://localhost:5000/appointment/search?doctorUserId=${userId}&diagnosis=${diagnostic}`);
            setAppointments(response.data);
        } catch (err) {
            console.error("Search failed:", err);
        }
    };

    const formatTime = (time) => {
        if (!time) return '';
        const [h, m] = time.split(':');
        return `${h.padStart(2, '0')}:${m.padStart(2, '0')}`;
    };

    const groupScheduleByDay = (schedule) => {
        const grouped = schedule.reduce((acc, entry) => {
            const day = entry.day;
            if (!acc[day]) acc[day] = [];
            acc[day].push(entry);
            return acc;
        }, {});

        Object.keys(grouped).forEach((day) => {
            grouped[day].sort((a, b) => {
                const [h1, m1] = a.startTime.split(':').map(Number);
                const [h2, m2] = b.startTime.split(':').map(Number);
                return h1 * 60 + m1 - (h2 * 60 + m2);
            });
        });

        return grouped;
    };

    const renderSchedule = () => {
        if (!doctor?.schedule) return <p>No schedule available.</p>;

        const grouped = groupScheduleByDay(doctor.schedule);
        const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

        return Object.entries(grouped).map(([day, entries]) => (
            <div key={day} className="mb-2">
                <h5>{days[day]}</h5>
                <ul className="list-disc pl-6">
                    {entries.map((entry, idx) => (
                        <li key={idx}>
                            {formatTime(entry.startTime)} - {formatTime(entry.endTime)}
                        </li>
                    ))}
                </ul>
            </div>
        ));
    };

    const handleInputChange = (id, field, value) => {
        setAppointments((prev) =>
            prev.map((appt) => appt.id === id ? { ...appt, [field]: value } : appt)
        );
    };

    const saveAppointment = async (id) => {
        const appointmentToUpdate = appointments.find((a) => a.id === id);
        try {
            setSaving(true);
            await axios.put(`http://localhost:5000/appointment/${id}`, appointmentToUpdate);
            alert('Appointment updated!');
        } catch (err) {
            console.error('Failed to save appointment:', err);
            alert('Error saving appointment.');
        } finally {
            setSaving(false);
        }
    };


    const handleScheduleChange = (index, field, value) => {
        setDoctor((prev) => {
            const newSchedule = [...prev.schedule];
            newSchedule[index][field] = value;
            return { ...prev, schedule: newSchedule };
        });
    };

    const addScheduleEntry = () => {
        setDoctor((prev) => ({
            ...prev,
            schedule: [...prev.schedule, { day: 0, startTime: "08:00", endTime: "10:00" }]
        }));
    };

    const removeScheduleEntry = (index) => {
        setDoctor((prev) => {
            const newSchedule = [...prev.schedule];
            newSchedule.splice(index, 1);
            return { ...prev, schedule: newSchedule };
        });
    };

    const saveSchedule = async () => {
        try {
            const payload = {
                userId: doctor.userId,
                fullName: doctor.fullName,
                cvPath: doctor.cvPath,
                photoUrl: doctor.photoUrl,
                specialties: doctor.specialties,
                schedule: doctor.schedule.map(entry => ({
                    day: parseInt(entry.day),
                    startTime: entry.startTime,
                    endTime: entry.endTime
                }))
            };

            await axios.put(`http://localhost:5000/doctor/${userId}`, payload);
            alert("Schedule saved!");
        } catch (err) {
            console.error("Failed to save schedule:", err);
            alert("Error saving schedule.");
        }
    };

    const renderEditableSchedule = () => (
        <div>
            <button className="btn btn-sm btn-success mb-2" onClick={addScheduleEntry}>+ Add Entry</button>
            {doctor.schedule.map((entry, index) => (
                <div key={index} className="d-flex align-items-center gap-2 mb-2">
                    <select
                        className="form-select w-25"
                        value={entry.day}
                        onChange={(e) => handleScheduleChange(index, 'day', e.target.value)}
                    >
                        {["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"].map((day, i) => (
                            <option key={i} value={i}>{day}</option>
                        ))}
                    </select>
                    <input
                        type="time"
                        className="form-control"
                        value={entry.startTime}
                        onChange={(e) => handleScheduleChange(index, 'startTime', e.target.value)}
                    />
                    <input
                        type="time"
                        className="form-control"
                        value={entry.endTime}
                        onChange={(e) => handleScheduleChange(index, 'endTime', e.target.value)}
                    />
                    <button className="btn btn-danger btn-sm" onClick={() => removeScheduleEntry(index)}>Remove</button>
                </div>
            ))}
            <button className="btn btn-primary mt-3" onClick={saveSchedule}>Save Schedule</button>
        </div>
    );

    if (loading) return <div className="p-6 text-center">Loading doctor dashboard...</div>;
    if (error) return <div className="p-6 text-red-600">{error}</div>;

    const contentMap = {
        Profile: (
            <div>
                <h2 className="text-2xl font-bold mb-3">{doctor.fullName}</h2>
                {photoUrl && <img src={photoUrl} alt="Doctor" style={{ width: '96px', height: '96px' }}  className="w-5 h-5 rounded object-cover mb-3" />}
                <p><strong>Specialties:</strong></p>
                <ul className="list-disc pl-6 mb-3">
                    {doctor.specialties.map((s, i) => <li key={i}>{s}</li>)}
                </ul>
                {cvUrl && (
                    <a href={cvUrl} className="btn btn-sm btn-secondary mt-2" download={`CV_${doctor.fullName}.pdf`} target="_blank" rel="noreferrer">
                        View/Download CV
                    </a>
                )}
            </div>
        ),
        Schedule: (
            <div>
                <h2 className="text-2xl font-bold mb-4">Schedule</h2>
                <h2 className="text-2xl font-bold mb-4">Edit Schedule</h2>
                {renderEditableSchedule()}
                {renderSchedule()}
            </div>
        ),
        Patients: (
            <div>
                <h2 className="text-2xl font-bold mb-4">Patients</h2>
                <h2>Search for Patients</h2>
                <input
                    type="text"
                    placeholder="Enter patient name"
                    value={searchPacientfullName}
                    onChange={(e) => setSearchPacientfullName(e.target.value)}
                />
                <button onClick={handleSearchPacient}>Search</button>

                <ul>
                    {patients.length > 0 ? (
                        patients.map((patient) => (
                            <li key={patient.id}>
                                <strong>{patient.fullName}</strong> (ID: {patient.id})
                            </li>
                        ))
                    ) : (
                        <p>No patients found</p>
                    )}
                </ul>
            </div>
        ),
        Appointments: (
            <div>
                <h2 className="text-2xl font-bold mb-4">Appointments</h2>
                <h4>Search Appointments</h4>
                <input
                    type="text"
                    placeholder="Diagnostic"
                    value={diagnostic}
                    onChange={(e) => setDiagnostic(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Tratament"
                    value={tratament}
                    onChange={(e) => setTratament(e.target.value)}
                />
                <button onClick={handleSearch}>Search</button>

                {appointments.length === 0 ? (
                    <p>No appointments found.</p>
                ) : (
                    <div className="table-responsive">
                        <table className="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Status</th>
                                    <th>Symptoms</th>
                                    <th>Diagnosis</th>
                                    <th>Treatment</th>
                                    <th>Observations</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {appointments.map((appt) => (
                                    <tr key={appt.id}>
                                        <td>{new Date(appt.appointmentDateTime).toLocaleString()}</td>
                                        <td>
                                            <select
                                                className="form-select"
                                                value={appt.status}
                                                onChange={(e) => handleInputChange(appt.id, 'status', e.target.value)}
                                            >
                                                <option value="Scheduled">Scheduled</option>
                                                <option value="Completed">Completed</option>
                                                <option value="Cancelled">Cancelled</option>
                                            </select>
                                        </td>
                                        <td>
                                            <input
                                                type="text"
                                                className="form-control"
                                                value={appt.symptoms || ''}
                                                onChange={(e) => handleInputChange(appt.id, 'symptoms', e.target.value)}
                                            />
                                        </td>
                                        <td>
                                            <input
                                                type="text"
                                                className="form-control"
                                                value={appt.diagnosis || ''}
                                                onChange={(e) => handleInputChange(appt.id, 'diagnosis', e.target.value)}
                                            />
                                        </td>
                                        <td>
                                            <input
                                                type="text"
                                                className="form-control"
                                                value={appt.treatment || ''}
                                                onChange={(e) => handleInputChange(appt.id, 'treatment', e.target.value)}
                                            />
                                        </td>
                                        <td>
                                            <input
                                                type="text"
                                                className="form-control"
                                                value={appt.observations || ''}
                                                onChange={(e) => handleInputChange(appt.id, 'observations', e.target.value)}
                                            />
                                        </td>
                                        <td>
                                            <button
                                                className="btn btn-primary btn-sm"
                                                onClick={() => saveAppointment(appt.id)}
                                                disabled={saving}
                                            >
                                                {saving ? 'Saving...' : 'Save'}
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        ),
    };

    return (
                <div className="container-fluid vh-100">
                    <div className="row h-100">
                        <div className="col-md-3 bg-light border-end" style={{ overflowY: 'auto' }}>
                            <h4 style={{ marginTop: '20px' }}>DoctorDashboard</h4>
                            <div className="list-group">
                                {Object.keys(contentMap).map((item) => (
                                    <a
                                        key={item}
                                        className={`list-group-item list-group-item-action ${activeTab === item ? 'active' : ''}`}
                                        onClick={() => setActiveTab(item)}
                                        style={{ cursor: 'pointer' }}
                                    >
                                        {item}
                                    </a>
                                ))}
                            </div>
                        </div>
                        <div className="col-md-9 p-4">
                            <div className="border p-3">
                        {activeTab ? contentMap[activeTab] : <p>Select an item to see the content.</p>}
                            </div>
                        </div>
                    </div>
                </div>
                
           
      
    );
};

export default DoctorDashboard;
