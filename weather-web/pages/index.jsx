import Head from "next/head";
import Image from "next/image";
import styles from "../styles/Home.module.css";

export default function Home() {
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
            type="text"
            className={styles.inputCity}
            placeholder="London"
          />
          <select className={styles.countrySelection}>
            <option value="uk">UK</option>
            <option value="aus">Australia</option>
          </select>
          <button className={styles.btnSearch}>Search</button>
        </div>

        <div className={styles.result}>
          <h2>Raining</h2>
        </div>
      </main>

      <footer className={styles.footer}>
<p></p>
      </footer>
    </div>
  );
}
