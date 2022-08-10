import { useState, useEffect } from "react";
import Head from "next/head";
import styles from "../styles/Home.module.css";

export default function Home() {
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState("");
  const [errorMsg, setErrorMsg] = useState("");
  const [city, setCity] = useState("");
  const [country, setCountry] = useState("au");
  const handleSearch = async (event) => {
    event.preventDefault();
    setLoading(true);
    setResult("");
    setErrorMsg("");
    try {
      const res = await fetch("http://localhost:5001/weather", {
        method: "post",
        headers: {
          "x-api-key": "ph4wr97efrASTOHasetaqICasIpRuc96",
          "content-type": "application/json",
        },
        body: JSON.stringify({
          city,
          country,
        }),
      });
      if (res.ok) {
        const data = await res.json();
        setResult(data.result);
      } else {
        setErrorMsg(res.statusText);
      }
    } catch (e) {
      setErrorMsg(e);
    }
    setLoading(false);
  };
  return (
    <div className={styles.container}>
      <Head>
        <title>Create Next App</title>
        <meta name="description" content="Weather web app" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main className={styles.main}>
        <h1>Weather</h1>
        <div className={styles.inputsWrapper}>
          <input
            name="city"
            type="text"
            className={styles.inputCity}
            placeholder="City"
            onChange={(e) => setCity(e.target.value)}
            value={city}
          />
          <select
            className={styles.countrySelection}
            onChange={(e) => setCountry(e.target.value)}
            value={country}
          >
            <option value="uk">UK</option>
            <option value="au">Australia</option>
          </select>
          <button
            className={styles.btnSearch}
            disabled={!country || !city || loading}
            onClick={handleSearch}
          >
            Search
          </button>
        </div>

        <div className={styles.result}>
          {loading ? (
            <p>loading</p>
          ) : errorMsg ? (
            <h3 style={{ color: "red" }}>{errorMsg}</h3>
          ) : result ? (
            <h2>{result}</h2>
          ) : (
            <div />
          )}
        </div>
      </main>

      <footer className={styles.footer}>
        <p></p>
      </footer>
    </div>
  );
}
